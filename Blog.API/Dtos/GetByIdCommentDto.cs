
namespace Blog.API.Dtos;

public class GetByIdCommentDto
{
    public int Id { get; set; }

    public string Text { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int PostId { get; set; }
    
    public int AuthorId { get; set; }
}