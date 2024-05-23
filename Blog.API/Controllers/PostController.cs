using AutoMapper;
using Blog.API.Dtos;
using Blog.Domain.Entities;
using Blog.Domain.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Blog.API.Controllers;

[ApiController]
[Route("[controller]")]
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
    public async Task<IActionResult> CreatePost(CreatePostDto createPost)
    {
        //assume that user is with id = 1
        //later we need to add jwt and get user id from token
        Post post = _mapper.Map<Post>(createPost);
        await _postRepo.PostPost(1, post);

        if (post == null)
            return NotFound("User not found");

        var mappedPost = _mapper.Map<GetByIdPostDto>(post);
        
        return Ok(mappedPost);
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> GetPostById(int id)
    {
        Post post = await _postRepo.GetById(id);
        
        if (post == null)
            return NotFound("User not found");
        
        return Ok(post);
    }
}