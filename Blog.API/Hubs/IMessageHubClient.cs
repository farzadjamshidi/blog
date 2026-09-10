namespace Blog.API.Hubs;

public interface IMessageHubClient
{
    Task NewComment(Guid postId, string commentText);
}
