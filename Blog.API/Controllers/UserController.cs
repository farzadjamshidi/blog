using AutoMapper;
using Blog.API.Dtos;
using Blog.Domain.Entities;
using Blog.Domain.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Blog.API.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserRepository _userRepo;
    private readonly IMapper _mapper;
    public UserController(
        IUserRepository userRepo,
        IMapper mapper
        )
    {
        _userRepo = userRepo;
        _mapper = mapper;
    }
    
    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> GetUserById(int id)
    {
        var user = await _userRepo.GetById(id);

        if (user == null)
            return NotFound("User not found");
        
        user.Picture = String.Concat("https://localhost:7163/", user.Picture);

        return Ok(user);
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