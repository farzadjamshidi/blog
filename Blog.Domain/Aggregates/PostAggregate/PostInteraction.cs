namespace Blog.Domain.Aggregates.PostAggregate;

public class PostInteraction
{
    private PostInteraction()
    {
        
    }
    public Guid Id { get; private set; }
    public Guid PostId { get; private set; }
    public InteractionType Type { get; private set; }

    public static PostInteraction CreatePostInteraction(Guid postId, InteractionType type)
    {
        return new PostInteraction()
        {
            PostId = postId,
            Type = type
        };
    }
}