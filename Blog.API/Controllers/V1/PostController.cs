using AutoMapper;
using Blog.API.Dtos.V1.Post.Requests;
using Blog.API.Dtos.V1.Post.Responses;
using Blog.Api.Extensions;
using Blog.Application.Caching;
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
    private static readonly TimeSpan CacheDuration = TimeSpan.FromSeconds(30);

    private readonly IMapper _mapper;
    private readonly IMediator _mediator;
    private readonly ICacheService _cacheService;

    public PostController(
        IMapper mapper,
        IMediator mediator,
        ICacheService cacheService
        )
    {
        _mapper = mapper;
        _mediator = mediator;
        _cacheService = cacheService;
    }

    // Comments/interactions can also change what this response contains
    // (via CreatePostComment, CreatePostInteraction, etc.), and those
    // handlers don't invalidate this cache entry — the 30s TTL bounds that
    // staleness instead of explicit invalidation from every handler that
    // touches a post's sub-collections.
    private static string CacheKey(Guid id) => $"post:{id}";

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var postsQuery = new GetAllPostQuery();
        var posts = await _mediator.Send(postsQuery);
        var response = _mapper.Map<List<CreatePostDtoRes>>(posts);
        return Ok(response);
    }

    // Same job as GetAll() above (list all posts), via Dapper + raw SQL
    // instead of EF Core + LINQ
    // Returns PostSummaryDapperDto directly, skipping the API-DTO/AutoMapper
    // hop GetAll() uses, since this is a small comparison endpoint.
    [HttpGet]
    [Route(Routes.Post.DapperSummary)]
    public async Task<IActionResult> GetAllDapper()
    {
        var posts = await _mediator.Send(new GetAllPostsDapperQuery());
        return Ok(posts);
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
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var cached = await _cacheService.GetAsync<GetPostByIdDtoRes>(CacheKey(id), cancellationToken);
        if (cached != null)
        {
            return Ok(cached);
        }

        var postQuery = new GetPostQuery()
        {
            Id = id
        };

        var post = await _mediator.Send(postQuery, cancellationToken);

        if (post == null)
        {
            return NotFound("Post not found");
        }

        var response = _mapper.Map<GetPostByIdDtoRes>(post);

        await _cacheService.SetAsync(CacheKey(id), response, CacheDuration, cancellationToken);

        return Ok(response);
    }

    [HttpPatch]
    [Route(Routes.Post.Entity)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdatePostDtoReq updatePostDtoReq, CancellationToken cancellationToken)
    {
        var updatePostCommand = _mapper.Map<UpdatePostCommand>(updatePostDtoReq);
        updatePostCommand.Id = id;

        var updatedPost = await _mediator.Send(updatePostCommand, cancellationToken);

        if (updatedPost == null)
        {
            return NotFound("Post not found");
        }

        await _cacheService.RemoveAsync(CacheKey(id), cancellationToken);

        return NoContent();
    }
    
    [HttpDelete]
    [Route(Routes.Post.Entity)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var deletePostCommand = new DeletePostCommand()
        {
            Id = id
        };

        var deletedPost = await _mediator.Send(deletePostCommand, cancellationToken);

        if (deletedPost == null)
        {
            return NotFound("Post not found");
        }

        await _cacheService.RemoveAsync(CacheKey(id), cancellationToken);

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