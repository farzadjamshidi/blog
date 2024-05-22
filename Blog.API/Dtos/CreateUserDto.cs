using System.ComponentModel.DataAnnotations;

namespace Blog.API.Dtos;

public class CreateUserDto
{
    [StringLength(50)]
    public string FirstName { get; set; }
    
    [StringLength(50)]
    public string LastName { get; set; }
    
    [StringLength(20)]
    public string Username { get; set; }
    
    [StringLength(64)]
    public string HashedPassword { get; set; }
}