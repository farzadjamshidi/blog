using System.ComponentModel.DataAnnotations;

namespace Blog.API.Dtos.Identity.Requests;

public class LoginUserDtoReq
{
    [EmailAddress]
    public string UserName { get; set; }
    public string Password { get; set; }
}