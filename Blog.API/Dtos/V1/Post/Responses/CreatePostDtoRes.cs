namespace Blog.API.Dtos.V1.Post.Responses;

public class CreatePostDtoRes
{
    public Guid Id { get; set; }
    public Domain.Aggregates.UserProfileAggregate.UserProfile UserProfile { get; set; }
    public string Text { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}