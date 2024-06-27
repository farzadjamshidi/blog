using Microsoft.AspNetCore.Mvc;

namespace Blog.API.Controllers.V1;

[ApiVersion("1.0")]
[ApiController]
[Route(Routes.Base)]
public class UserProfileController : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok();
    }
    
    [HttpGet]
    [Route(Routes.UserProfile.Entity)]
    public async Task<IActionResult> GetById()
    {
        return Ok();
    }
    
    [HttpPost]
    public async Task<IActionResult> Create()
    {
        return Ok();
    }
}