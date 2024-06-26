using System.Net.Mime;

namespace Blog.Domain.Aggregates.PostAggregate;

public class PostComment
{
    private PostComment()
    {
    }
    public Guid Id { get; private set; }
    public Guid PostId { get; private set; }
    public Guid UserProfileId { get; private set; }
    public string Text { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    public static PostComment CreatePostComment(Guid postId, Guid userProfileId, string text)
    {
        return new PostComment()
        {
            PostId = postId,
            UserProfileId = userProfileId,
            Text = text,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }

    public void UpdateCommentText(string newText)
    {
        Text = newText;
        UpdatedAt = DateTime.UtcNow;
    }
}