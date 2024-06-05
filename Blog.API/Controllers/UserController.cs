using System.Text.Json;
using AutoMapper;
using Blog.API.Dtos;
using Blog.Domain.Entities;
using Blog.Domain.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace Blog.API.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserRepository _userRepo;
    private readonly IMapper _mapper;
    private readonly IDistributedCache _distributedCache;
    public UserController(
        IDistributedCache distributedCache,
        IUserRepository userRepo,
        IMapper mapper
        )
    {
        _distributedCache = distributedCache;
        _userRepo = userRepo;
        _mapper = mapper;
    }
    
    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> GetUserById(int id)
    {
        var cacheKey = $"user:{id}";
        var userJson = await _distributedCache.GetStringAsync(cacheKey);
        if (userJson is null)
        {
            var user = await _userRepo.GetById(id);
            if (user == null)
                return NotFound("User not found");
        
            user.Picture = String.Concat("https://localhost:7163/", user.Picture);

            await _distributedCache.SetStringAsync(cacheKey, JsonSerializer.Serialize(user));
            
            return Ok(user);
        }

        return Ok(JsonSerializer.Deserialize<User>(userJson));
    }
    
    [HttpPost]
    [Route("")]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserDto createUser)
    {
        User user = _mapper.Map<User>(createUser);
        await _userRepo.PostUser(user);
        
        return Ok(user);
    }
}