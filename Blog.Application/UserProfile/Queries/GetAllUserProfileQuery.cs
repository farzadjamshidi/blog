using MediatR;

namespace Blog.Application.UserProfile.Queries;

public class GetAllUserProfileQuery : IRequest<IEnumerable<Domain.Aggregates.UserProfileAggregate.UserProfile>>
{
    
}