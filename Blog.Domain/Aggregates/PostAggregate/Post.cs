
using System.Net.Mime;
using Blog.Domain.Aggregates.UserProfileAggregate;

namespace Blog.Domain.Aggregates.PostAggregate;

public class Post
{
    private readonly List<PostComment> _comments = new List<PostComment>();
    private readonly List<PostInteraction> _interactions = new List<PostInteraction>();
    private Post()
    {
    }
    public Guid Id { get; private set; }
    public Guid UserProfileId { get; private set; }
    public UserProfile UserProfile { get; private set; }
    public string Text { get; private set; }
    public IEnumerable<PostComment> Comments
    {
        get { return _comments; }
    }
    public IEnumerable<PostInteraction> Interactions
    {
        get { return _interactions; }
    }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    public static Post CreatePost(Guid userProfileId, string text)
    {
        //Here is for validations
        return new Post()
        {
            UserProfileId = userProfileId,
            Text = text,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }

    public void UpdatePostText(string text)
    {
        Text = text;
        UpdatedAt = DateTime.UtcNow;
    }

    public void AddComment(PostComment newPostComment)
    {
        //Here is for validations
        _comments.Add(newPostComment);
    }

    public void RemoveComment(PostComment postComment)
    {
        _comments.Remove(postComment);
    }
    
    public void AddInteraction(PostInteraction postInteraction)
    {
        //Here is for validations
        _interactions.Add(postInteraction);
    }

    public void RemoveInteraction(PostInteraction postInteraction)
    {
        _interactions.Remove(postInteraction);
    }
}