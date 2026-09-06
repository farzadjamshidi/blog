namespace Blog.Application.Dtos.Post;

public class PostSummaryDapperDto
{
    public Guid Id { get; set; }
    public string Text { get; set; }
    public DateTime CreatedAt { get; set; }
}
