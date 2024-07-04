namespace Blog.API.Dtos.V1.Post.Responses;

public class CreatePostCommentDtoRes
{
    public Guid Id { get; set; }
    public Guid PostId { get; set; }
    public Guid UserProfileId { get; set; }
    public string Text { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}