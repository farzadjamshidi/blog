using AutoMapper;
using Blog.API.Dtos.Identity.Requests;
using Blog.API.Dtos.Identity.Responses;
using Blog.Application.Identity.Commands;
using Blog.Application.Identity.Query;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Blog.API.Controllers.V1;

[ApiVersion("1.0")]
[ApiController]
[Route(Routes.Base)]
public class IdentityController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public IdentityController(
        IMapper mapper, 
        IMediator mediator
        )
    {
        _mapper = mapper;
        _mediator = mediator;
    }


    [HttpPost]
    [Route(Routes.Identity.Register)]
    public async Task<IActionResult> Register([FromBody] RegisterUserDtoReq registerUserDtoReq, 
        CancellationToken cancellationToken)
    {
        var registerUserCommand = _mapper.Map<RegisterUserCommand>(registerUserDtoReq);
        var registerUserCommandHandlerDto = await _mediator.Send(registerUserCommand, cancellationToken);

        if (registerUserCommandHandlerDto == null)
        {
            return BadRequest("Something is wrong. Please try again.");
        }
        
        var response = _mapper.Map<RegisterUserDtoRes>(registerUserCommandHandlerDto);
       
        return Ok(response);
    }

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login([FromBody] LoginUserDtoReq loginUserDtoReq, CancellationToken cancellationToken)
    {
        var loginUserQuery = new LoginUserQuery
        {
            UserName = loginUserDtoReq.UserName,
            Password = loginUserDtoReq.Password
        };
        
        var loginUserCommandHandlerDto = await _mediator.Send(loginUserQuery, cancellationToken);

        if (loginUserCommandHandlerDto == null)
        {
            return BadRequest("Something is wrong. Please try again.");
        }
        
        var response = _mapper.Map<RegisterUserDtoRes>(loginUserCommandHandlerDto);
       
        return Ok(response);
    }
}