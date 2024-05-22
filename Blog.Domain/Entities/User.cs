using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blog.Domain.Entities;

[Table("Users")]
public class User
{
    public int Id { get; set; }
    
    [StringLength(50)]
    public string FirstName { get; set; }
    
    [StringLength(50)]
    public string LastName { get; set; }
    
    [StringLength(20)]
    public string Username { get; set; }
    
    [StringLength(64)]
    public string HashedPassword { get; set; }
}