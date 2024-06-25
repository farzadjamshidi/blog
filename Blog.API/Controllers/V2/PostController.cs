using Microsoft.AspNetCore.Mvc;

namespace Blog.API.Controllers.V2;

[ApiVersion("2.0")]
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public class PostController : ControllerBase
{
    public PostController()
    {
    }
    
    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> GetPostById(int id)
    {
        return Ok("Post controller version 2");
    }
}