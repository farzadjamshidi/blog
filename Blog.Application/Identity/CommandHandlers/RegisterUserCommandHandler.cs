using System.Security.Claims;
using Blog.Application.Identity.Commands;
using Blog.Application.Identity.Dtos;
using Blog.Application.Identity.Services;
using Blog.DAL;
using Blog.Domain.Aggregates.UserProfileAggregate;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.JsonWebTokens;

namespace Blog.Application.Identity.CommandHandlers;

public class RegisterUserCommandHandler: IRequestHandler<RegisterUserCommand, RegisterUserCommandHandlerDto?>
{
    private readonly DataContext _ctx;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IdentityService _identityService;

    public RegisterUserCommandHandler(
        UserManager<IdentityUser> userManager,
        IdentityService identityService,
        SignInManager<IdentityUser> signInManager, 
        DataContext ctx
        )
    {
        _userManager = userManager;
        _identityService = identityService;
        _ctx = ctx;
    }
    
    public async Task<RegisterUserCommandHandlerDto?> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var existingIdentity = await _userManager.FindByEmailAsync(request.UserName);

        if (existingIdentity != null) return null;

        await using var transaction = await _ctx.Database.BeginTransactionAsync(cancellationToken);
        
        var identity = new IdentityUser
        {
            Email = request.UserName,
            UserName = request.UserName
        };
        
        var createdIdentity = await _userManager.CreateAsync(identity, request.Password);

        if (!createdIdentity.Succeeded)
        {
            await transaction.RollbackAsync(cancellationToken);
            return null;
        }
        
        var basicInfo = BasicInfo.CreateBasicInfo(request.FirstName, request.LastName, request.UserName,
            request.Phone, request.DateOfBirth, request.CurrentCity);

        var userProfile = Blog.Domain.Aggregates.UserProfileAggregate.UserProfile
            .CreateUserProfile(identity.Id, basicInfo);
        
        try
        {
            _ctx.UserProfiles.Add(userProfile);
            await _ctx.SaveChangesAsync(cancellationToken);
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            return null;
        }
        
        await transaction.CommitAsync(cancellationToken);
        
        var claimsIdentity = new ClaimsIdentity(new Claim[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, identity.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Email, identity.Email),
            new Claim("IdentityId", identity.Id),
            new Claim("UserProfileId", userProfile.Id.ToString())
        });
        
        var token = _identityService.CreateSecurityToken(claimsIdentity);
        
        return new RegisterUserCommandHandlerDto()
        {
            Token = _identityService.WriteToken(token)
        };
        
        
    }
}

 