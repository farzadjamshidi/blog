using System.Security.Claims;
using AutoMapper;
using Blog.API.Dtos;
using Blog.API.Hub;
using Blog.Domain.Entities;
using Blog.Domain.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Blog.API.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class CommentController : ControllerBase
{
    private readonly IPostRepository _postRepo;
    private readonly ICommentRepository _commentRepo;
    private readonly IMapper _mapper;
    private readonly IHubContext < MessageHub, IMessageHubClient > _messageHub;
    public CommentController(
        IPostRepository postRepo,
        ICommentRepository commentRepo,
        IMapper mapper,
        IHubContext < MessageHub, IMessageHubClient > messageHub
        )
    {
        _postRepo = postRepo;
        _commentRepo = commentRepo;
        _mapper = mapper;
        _messageHub = messageHub;
    }
    
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCommentDto createComment)
    {
        int userId =Int32.Parse((User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value));
        
        Comment comment = _mapper.Map<Comment>(createComment);
        comment.AuthorId = userId;

        await _commentRepo.Post(comment.PostId, comment);

        var mappedComment = _mapper.Map<GetByIdCommentDto>(comment);
        
        await _messageHub.Clients.User(comment.Post.AuthorId.ToString()).NewComment($"comment added for post '{comment.Post.Content}' by '{comment.Post.Author.LastName}'");

        return Ok(mappedComment);
    }

    [HttpDelete]
    [Route("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        int userId =Int32.Parse((User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value));

        Comment comment = await _commentRepo.GetById(id);

        if (comment.AuthorId != userId)
            return Unauthorized("The user is not the post author");
        
        await _commentRepo.Delete(id);

        return NoContent();
    }
}