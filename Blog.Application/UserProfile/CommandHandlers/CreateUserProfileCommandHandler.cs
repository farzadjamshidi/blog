using Blog.Application.UserProfile.Commands;
using Blog.DAL;
using Blog.Domain.Aggregates.UserProfileAggregate;
using MediatR;

namespace Blog.Application.UserProfile.CommandHandlers;

public class CreateUserProfileCommandHandler: IRequestHandler<CreateUserProfileCommand, Domain.Aggregates.UserProfileAggregate.UserProfile>
{
    private readonly DataContext _ctx;

    public CreateUserProfileCommandHandler(DataContext ctx)
    {
        _ctx = ctx;
    }
    
    public async Task<Domain.Aggregates.UserProfileAggregate.UserProfile> Handle(CreateUserProfileCommand request, CancellationToken cancellationToken)
    {
        var basicInfo = BasicInfo.CreateBasicInfo(request.FirstName, request.LastName, request.EmailAddress,
            request.Phone, request.DateOfBirth, request.CurrentCity);
        var userProfile =
            Domain.Aggregates.UserProfileAggregate.UserProfile.CreateUserProfile(Guid.NewGuid().ToString(), basicInfo);
        _ctx.UserProfiles.Add(userProfile);
        await _ctx.SaveChangesAsync(cancellationToken);

        return userProfile;
    }
}