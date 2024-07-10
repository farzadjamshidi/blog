using Blog.Application.Identity.Dtos;
using MediatR;

namespace Blog.Application.Identity.Commands;

public class RegisterUserCommand: IRequest<RegisterUserCommandHandlerDto?>
{
    public string UserName { get; set; }
    public string Password { get; set; }

    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Phone { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string CurrentCity { get; set; }
}