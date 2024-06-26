using System.Security.Claims;
using AutoMapper;
using Blog.API.Dtos;
using Blog.Domain.Entities;
using Blog.Domain.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.API.Controllers.V1;

[ApiVersion("1.0")]
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[Authorize]
public class PostController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IPostRepository _postRepo;
    public PostController(
        IMapper mapper,
        IPostRepository postRepo
        )
    {
        _mapper = mapper;
        _postRepo = postRepo;
    }
    
    [HttpPost]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> CreatePost(CreatePostDto createPost)
    {
        int userId =Int32.Parse((User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value));

        Post post = _mapper.Map<Post>(createPost);
        await _postRepo.PostPost(userId, post);

        if (post == null)
            return NotFound("User not found");

        var mappedPost = _mapper.Map<GetByIdPostDto>(post);
        
        return Ok(mappedPost);
    }

    // [MapToApiVersion("2.0")] we prefer to not use this approach to have a cleaner code.
    [HttpGet]
    [Route("{id}")]
    [Authorize(Roles = "Administrator, User")]
    public async Task<IActionResult> GetPostById(int id)
    {
        Post post = await _postRepo.GetById(id);
        
        if (post == null)
            return NotFound("User not found");
        
        return Ok(post);
    }
}