using MediatR;

namespace Blog.Application.UserProfile.Queries;

public class GetUserProfileQuery: IRequest<Domain.Aggregates.UserProfileAggregate.UserProfile?>
{
    public Guid Id { get; set; }
}