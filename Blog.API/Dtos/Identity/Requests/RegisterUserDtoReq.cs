using System.ComponentModel.DataAnnotations;

namespace Blog.API.Dtos.Identity.Requests;

public class RegisterUserDtoReq
{
    [EmailAddress]
    public string UserName { get; set; }
    public string Password { get; set; }

    public string FirstName { get; set; }
    public string LastName { get; set; }
    
    public string Phone { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string CurrentCity { get; set; }
}