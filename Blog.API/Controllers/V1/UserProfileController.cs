using AutoMapper;
using Blog.API.Dtos.V1.UserProfile.Requests;
using Blog.API.Dtos.V1.UserProfile.Responses;
using Blog.Application.UserProfile.Commands;
using Blog.Application.UserProfile.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Blog.API.Controllers.V1;

[ApiVersion("1.0")]
[ApiController]
[Route(Routes.Base)]
public class UserProfileController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public UserProfileController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var userProfilesQuery = new GetAllUserProfileQuery();

        var userProfiles = await _mediator.Send(userProfilesQuery);
        
        var response = _mapper.Map<List<CreateUserProfileDtoRes>>(userProfiles);
        
        return Ok(response);
    }
    
    [HttpGet]
    [Route(Routes.UserProfile.Entity)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var userProfileQuery = new GetUserProfileQuery()
        {
            Id = id
        };
        var userProfile = await _mediator.Send(userProfileQuery);

        if (userProfile == null)
        {
            return NotFound("User profile not found");
        }
        
        var response = _mapper.Map<CreateUserProfileDtoRes>(userProfile);
        
        return Ok(response);
    }
    
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateUserProfileDtoReq userProfileDtoReq)
    {
        var userProfileCommand = _mapper.Map<CreateUserProfileCommand>(userProfileDtoReq);
        var userProfile = await _mediator.Send(userProfileCommand);

        var response = _mapper.Map<CreateUserProfileDtoRes>(userProfile);
        
        return CreatedAtAction(nameof(GetById), new {id = userProfile.Id}, response);
    }

    [HttpPatch]
    [Route(Routes.UserProfile.Entity)]
    public async Task<IActionResult> Update(Guid id, [FromBody] CreateUserProfileDtoReq updateUserProfileDtoReq)
    {
        var updateUserProfileCommand = _mapper.Map<UpdateUserProfileCommand>(updateUserProfileDtoReq);
        updateUserProfileCommand.Id = id;
        
        var updatedUserProfile = await _mediator.Send(updateUserProfileCommand);
        
        if (updatedUserProfile == null)
        {
            return NotFound("User profile not found");
        }
        
        return NoContent();
    }

    [HttpDelete]
    [Route(Routes.UserProfile.Entity)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var deleteUserProfileQuery = new DeleteUserProfileCommand()
        {
            Id = id
        };
        var userProfile = await _mediator.Send(deleteUserProfileQuery);

        if (userProfile == null)
        {
            return NotFound("User profile not found");
        }
        
        return NoContent();
    }
}