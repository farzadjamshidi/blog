using System.Security.Claims;
using AutoMapper;
using Blog.API.Dtos;
using Blog.Domain.Entities;
using Blog.Domain.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.API.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class CommentController : ControllerBase
{
    private readonly IPostRepository _postRepo;
    private readonly ICommentRepository _commentRepo;
    private readonly IMapper _mapper;
    
    public CommentController(
        IPostRepository postRepo,
        ICommentRepository commentRepo,
        IMapper mapper
        )
    {
        _postRepo = postRepo;
        _commentRepo = commentRepo;
        _mapper = mapper;
    }
    
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCommentDto createComment)
    {
        int userId =Int32.Parse((User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value));
        
        Comment comment = _mapper.Map<Comment>(createComment);
        comment.AuthorId = userId;

        await _commentRepo.Post(comment.PostId, comment);

        var mappedComment = _mapper.Map<GetByIdCommentDto>(comment);

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