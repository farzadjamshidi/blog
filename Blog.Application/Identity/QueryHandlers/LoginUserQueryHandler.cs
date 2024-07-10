using System.Security.Claims;
using Blog.Application.Identity.Dtos;
using Blog.Application.Identity.Query;
using Blog.Application.Identity.Services;
using Blog.DAL;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;

namespace Blog.Application.Identity.QueryHandlers;

public class LoginUserQueryHandler: IRequestHandler<LoginUserQuery, RegisterUserCommandHandlerDto?>
{
    private readonly DataContext _ctx;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly IdentityService _identityService;

    public LoginUserQueryHandler(
        UserManager<IdentityUser> userManager,
        IdentityService identityService,
        SignInManager<IdentityUser> signInManager, 
        DataContext ctx
    )
    {
        _userManager = userManager;
        _identityService = identityService;
        _signInManager = signInManager;
        _ctx = ctx;
    }
    
    public async Task<RegisterUserCommandHandlerDto?> Handle(LoginUserQuery request, CancellationToken cancellationToken)
    {
        var identity = await _userManager.FindByEmailAsync(request.UserName);
        if (identity == null) return null;
        
        var result = await _signInManager.CheckPasswordSignInAsync(identity, request.Password, false);
        
        if (!result.Succeeded) return null;
        
        var userProfile = await _ctx.UserProfiles
            .FirstOrDefaultAsync(up => up.IdentityId == identity.Id, cancellationToken:
                cancellationToken);
    
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