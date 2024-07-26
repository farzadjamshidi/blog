using Blog.Domain.Aggregates.PostAggregate;

namespace Blog.API.Dtos.V1.Post.Responses;

public class GetPostByIdDtoRes
{
    public Guid Id { get; set; }
    public Guid UserProfileId { get; set; }
    public string Text { get; set; }
    public List<PostComment> Comments { get; set; }
    public List<InteractionCount> InteractionsCount { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class InteractionCount
{
    public InteractionType Type { get; set; }
    public int Count { get; set; }
}