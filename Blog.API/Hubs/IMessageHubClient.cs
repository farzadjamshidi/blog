namespace Blog.API.Hubs;

public interface IMessageHubClient {
    Task NewComment(string message);
}