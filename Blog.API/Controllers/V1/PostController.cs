using AutoMapper;
using Blog.API.Dtos.V1.Post.Requests;
using Blog.API.Dtos.V1.Post.Responses;
using Blog.Application.Post.Commands;
using Blog.Application.Post.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Blog.API.Controllers.V1;

[ApiVersion("1.0")]
[ApiController]
[Route(Routes.Base)]
public class PostController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public PostController(
        IMapper mapper, 
        IMediator mediator
        )
    {
        _mapper = mapper;
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var postsQuery = new GetAllPostQuery();
        var posts = await _mediator.Send(postsQuery);
        var response = _mapper.Map<List<CreatePostDtoRes>>(posts);
        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreatePostDtoReq createPostDtoReq)
    {
        
        var postCommand = _mapper.Map<CreatePostCommand>(createPostDtoReq);

        var post = await _mediator.Send(postCommand);

        var response = _mapper.Map<CreatePostDtoRes>(post);

        return CreatedAtAction(nameof(GetById), new {id = post.Id}, response);
    }

    // [MapToApiVersion("2.0")] we prefer to not use this approach to have a cleaner code.
    [HttpGet]
    [Route(Routes.Post.Entity)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var postQuery = new GetPostQuery()
        {
            Id = id
        };
        
        var post = await _mediator.Send(postQuery);
        
        if (post == null)
        {
            return NotFound("Post not found");
        }
        
        var response = _mapper.Map<CreatePostDtoRes>(post);
        
        return Ok(response);
    }

    [HttpPatch]
    [Route(Routes.Post.Entity)]
    public async Task<IActionResult> Update(Guid id, UpdatePostDtoReq updatePostDtoReq)
    {
        var updatePostCommand = _mapper.Map<UpdatePostCommand>(updatePostDtoReq);
        updatePostCommand.Id = id;
        
        var updatedPost = await _mediator.Send(updatePostCommand);

        if (updatedPost == null)
        {
            return NotFound("Post not found");
        }
        
        return NoContent();
    }
    
    [HttpDelete]
    [Route(Routes.Post.Entity)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var deletePostCommand = new DeletePostCommand()
        {
            Id = id
        };
        
        var deletedPost = await _mediator.Send(deletePostCommand);

        if (deletedPost == null)
        {
            return NotFound("Post not found");
        }
        
        return NoContent();
    }
}