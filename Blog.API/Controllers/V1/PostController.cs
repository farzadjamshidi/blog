using AutoMapper;
using Blog.API.Dtos.V1.Post.Requests;
using Blog.API.Dtos.V1.Post.Responses;
using Blog.Api.Extensions;
using Blog.Application.Post.Commands;
using Blog.Application.Post.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.API.Controllers.V1;

[ApiVersion("1.0")]
[ApiController]
[Route(Routes.Base)]
[Authorize()]
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
        var postCommand = new CreatePostCommand
        {
            UserProfileId = HttpContext.GetUserProfileIdClaimValue(),
            Text = createPostDtoReq.Text
        };

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
        
        var response = _mapper.Map<GetPostByIdDtoRes>(post);
        
        return Ok(response);
    }

    [HttpPatch]
    [Route(Routes.Post.Entity)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdatePostDtoReq updatePostDtoReq)
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

    [HttpGet]
    [Route(Routes.Post.Comment)]
    public async Task<IActionResult> GetAllComments(Guid id, CancellationToken cancellationToken)
    {
        var postCommentsQuery = new GetAllPostCommentQuery()
        {
            PostId = id
        };

        var postComments = await _mediator.Send(postCommentsQuery, cancellationToken);

        if (postComments == null)
        {
            return NotFound("Post not found");
        }

        var response = _mapper.Map<List<CreatePostCommentDtoRes>>(postComments);

        return Ok(response);
    }

    [HttpPost]
    [Route(Routes.Post.Comment)]
    public async Task<IActionResult> CreateComment(
        Guid id, [FromBody] CreatePostCommentDtoReq createPostCommentDtoReq, CancellationToken cancellationToken)
    {
        var createPostCommentCommand = new CreatePostCommentCommand()
        {
            PostId = id,
            UserProfileId = HttpContext.GetUserProfileIdClaimValue(),
            Text = createPostCommentDtoReq.Text
        };

        var postComment = await _mediator.Send(createPostCommentCommand, cancellationToken);
        
        if (postComment == null)
        {
            return NotFound("Post not found");
        }

        var response = _mapper.Map<CreatePostCommentDtoRes>(postComment);

        return Ok(response);
    }
    
    [HttpDelete]
    [Route(Routes.Post.CommentEntity)]
    public async Task<IActionResult> DeleteComment(Guid id, Guid commentId, 
        CancellationToken cancellationToken)
    {
        var deletePostCommentCommand = new DeletePostCommentCommand()
        {
            PostId = id,
            CommentId = commentId
        };
        
        var deletedPostComment = await _mediator.Send(deletePostCommentCommand, cancellationToken);

        if (deletedPostComment == null)
        {
            return NotFound("Post not found");
        }
        
        return NoContent();
    }

    [HttpGet]
    [Route(Routes.Post.Interaction)]
    public async Task<IActionResult> GetAllInteractions(Guid id, CancellationToken cancellationToken)
    {
        var postInteractionsQuery = new GetAllPostInteractionsQuery()
        {
            PostId = id
        };
        
        var postInteractions = await _mediator.Send(postInteractionsQuery, cancellationToken);

        if (postInteractions == null)
        {
            return NotFound("Post not found");
        }

        var response = _mapper.Map<List<CreatePostInteractionDtoRes>>(postInteractions);

        return Ok(response);
    }
    
    [HttpPost]
    [Route(Routes.Post.Interaction)]
    public async Task<IActionResult> CreateInteraction(
        Guid id, [FromBody] CreatePostInteractionDtoReq createPostInteractionDtoReq, CancellationToken cancellationToken)
    {
        var createPostInteractionCommand = new CreatePostInteractionCommand()
        {
            PostId = id,
            Type = createPostInteractionDtoReq.Type
        };

        var postInteraction = await _mediator.Send(createPostInteractionCommand, cancellationToken);
        
        if (postInteraction == null)
        {
            return NotFound("Post not found");
        }

        var response = _mapper.Map<CreatePostInteractionDtoRes>(postInteraction);

        return Ok(response);
    }
    
    [HttpDelete]
    [Route(Routes.Post.InteractionEntity)]
    public async Task<IActionResult> DeleteInteraction(Guid id, Guid interactionId, 
        CancellationToken cancellationToken)
    {
        var deletePostInteractionCommand = new DeletePostInteractionCommand()
        {
            PostId = id,
            InteractionId = interactionId
        };
        
        var deletedPostInteraction = await _mediator.Send(deletePostInteractionCommand, cancellationToken);

        if (deletedPostInteraction == null)
        {
            return NotFound("Post not found");
        }
        
        return NoContent();
    }
}