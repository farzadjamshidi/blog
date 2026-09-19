using Blog.Domain.Aggregates.PostAggregate;

namespace Blog.Application.Dtos.Post;

public class CreatePostCommentResult
{
    public PostComment Comment { get; set; }
    public Guid PostAuthorUserProfileId { get; set; }
}
