using Blog.Application.UserProfile.Commands;
using Blog.DAL;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Blog.Application.UserProfile.CommandHandlers;

public class DeleteUserProfileCommandHandler: IRequestHandler<DeleteUserProfileCommand, Domain.Aggregates.UserProfileAggregate.UserProfile?>
{
    private readonly DataContext _ctx;

    public DeleteUserProfileCommandHandler(DataContext ctx)
    {
        _ctx = ctx;
    }
    public async Task<Domain.Aggregates.UserProfileAggregate.UserProfile?> Handle(DeleteUserProfileCommand request, CancellationToken cancellationToken)
    {
        var userprofile = await _ctx.UserProfiles.FirstOrDefaultAsync(up => up.Id == request.Id, cancellationToken);

        if (userprofile == null)
        {
            return null;
        }
        
        _ctx.UserProfiles.Remove(userprofile);
        await _ctx.SaveChangesAsync(cancellationToken);
        
        return userprofile;
    }
}