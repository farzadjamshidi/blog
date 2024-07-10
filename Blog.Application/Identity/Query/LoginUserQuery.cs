using Blog.Application.Identity.Dtos;
using MediatR;

namespace Blog.Application.Identity.Query;

public class LoginUserQuery: IRequest<RegisterUserCommandHandlerDto?>
{
    public string UserName { get; set; }
    public string Password { get; set; }
}