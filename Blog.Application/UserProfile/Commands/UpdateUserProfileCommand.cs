using Blog.Domain.Aggregates.UserProfileAggregate;
using MediatR;

namespace Blog.Application.UserProfile.Commands;

public class UpdateUserProfileCommand : IRequest<Domain.Aggregates.UserProfileAggregate.UserProfile?>
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string EmailAddress { get; set; }
    public string Phone { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string CurrentCity { get; set; }
}