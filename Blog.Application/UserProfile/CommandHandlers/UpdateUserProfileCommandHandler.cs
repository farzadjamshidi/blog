using Blog.Application.UserProfile.Commands;
using Blog.DAL;
using Blog.Domain.Aggregates.UserProfileAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Blog.Application.UserProfile.CommandHandlers;

public class UpdateUserProfileCommandHandler : IRequestHandler<UpdateUserProfileCommand, Domain.Aggregates.UserProfileAggregate.UserProfile?>
{
    private readonly DataContext _ctx;

    public UpdateUserProfileCommandHandler(DataContext ctx)
    {
        _ctx = ctx;
    }
    
    public async Task<Domain.Aggregates.UserProfileAggregate.UserProfile?> Handle(UpdateUserProfileCommand request, CancellationToken cancellationToken)
    {
        var updatedBasicInfo = BasicInfo.CreateBasicInfo(request.FirstName, request.LastName, request.EmailAddress,
            request.Phone, request.DateOfBirth, request.CurrentCity);

        var userprofile = await _ctx.UserProfiles.FirstOrDefaultAsync(up => up.Id == request.Id, cancellationToken);

        if (userprofile == null)
        {
            return null;
        }

        userprofile.UpdateBasicInfo(updatedBasicInfo);

        _ctx.UserProfiles.Update(userprofile);
        await _ctx.SaveChangesAsync(cancellationToken);
        
        return userprofile;
    }
}