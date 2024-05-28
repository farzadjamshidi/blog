using System.ComponentModel.DataAnnotations.Schema;

namespace Blog.Domain.Entities;

public class Post
{
    public int Id { get; set; }

    [ForeignKey(nameof(Author))]
    public int AuthorId { get; set; }
    public virtual User Author { get; set; }
    public string Content { get; set; }
    
    public List<Comment> Comments { get; set; }

    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}