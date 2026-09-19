using Blog.API.Dtos.V1.UserProfile.Responses;
using Blog.Domain.Aggregates.PostAggregate;

namespace Blog.API.Dtos.V1.Post.Responses;

public class GetPostByIdDtoRes
{
    public Guid Id { get; set; }
    public CreateUserProfileDtoRes UserProfile { get; set; }
    public string Text { get; set; }
    public List<PostCommentDtoRes> Comments { get; set; }
    public List<InteractionCount> InteractionsCount { get; set; }
    public int EngagementScore { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

// Cached via ICacheService (4.4) — must be a real DTO, not the domain
// PostComment. Domain entities use a private constructor + factory method
// (DDD pattern), which System.Text.Json can serialize but never
// deserialize back, breaking every cache hit
// (learning-notes/notes/19-caching.md).
public class PostCommentDtoRes
{
    public Guid Id { get; set; }
    public Guid PostId { get; set; }
    public CreateUserProfileDtoRes UserProfile { get; set; }
    public string Text { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class InteractionCount
{
    public InteractionType Type { get; set; }
    public int Count { get; set; }
    public int Weight { get; set; }
}
