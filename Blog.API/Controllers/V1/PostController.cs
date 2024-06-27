using System.Security.Claims;
using AutoMapper;
using Blog.API.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.API.Controllers.V1;

[ApiVersion("1.0")]
[ApiController]
[Route(Routes.Base)]
[Authorize]
public class PostController : ControllerBase
{
    private readonly IMapper _mapper;
    public PostController(
        IMapper mapper
        )
    {
        _mapper = mapper;
    }
    
    [HttpPost]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> CreatePost(CreatePostDto createPost)
    {
        return Ok();
    }

    // [MapToApiVersion("2.0")] we prefer to not use this approach to have a cleaner code.
    [HttpGet]
    [Route(Routes.Post.Entity)]
    [Authorize(Roles = "Administrator, User")]
    public async Task<IActionResult> GetPostById(int id)
    {
        return Ok();
    }
}