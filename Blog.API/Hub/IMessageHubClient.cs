namespace Blog.API.Hub;

public interface IMessageHubClient {
    Task NewComment(string message);
}