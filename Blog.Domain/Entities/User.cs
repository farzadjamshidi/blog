using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Blog.Domain.Entities;

public class User : IdentityUser<int>
{
    public User()
    {
        this.SecurityStamp = Guid.NewGuid().ToString();
    }
    
    [StringLength(50)]
    public string FirstName { get; set; }
    
    [StringLength(50)]
    public string LastName { get; set; }
    
    public List<Post> Posts { get; set; }
}