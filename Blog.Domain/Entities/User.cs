using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Blog.Domain.Entities;

[Table("Users")]
public class User : IdentityUser<int>
{
    public new int Id { get; set; }
    
    [StringLength(50)]
    public string FirstName { get; set; }
    
    [StringLength(50)]
    public string LastName { get; set; }
    
    [StringLength(20)]
    public string Username { get; set; }
    
    [StringLength(64)]
    public string HashedPassword { get; set; }
    
    public List<Post> Posts { get; set; }
}