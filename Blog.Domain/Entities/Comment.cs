using System.ComponentModel.DataAnnotations.Schema;

namespace Blog.Domain.Entities;

public class Comment
{
    public int Id { get; set; }

    public string Text { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    [ForeignKey(nameof(Post))]
    public int PostId { get; set; }
    public virtual Post Post { get; set; }
    
    [ForeignKey(nameof(Author))]
    public int AuthorId { get; set; }
    public virtual User Author { get; set; }
}