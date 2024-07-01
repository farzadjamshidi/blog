using MediatR;

namespace Blog.Application.UserProfile.Commands;

public class DeleteUserProfileCommand: IRequest<Domain.Aggregates.UserProfileAggregate.UserProfile?>
{
    public Guid Id { get; set; }
}