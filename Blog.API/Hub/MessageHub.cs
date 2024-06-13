using Microsoft.AspNetCore.SignalR;

namespace Blog.API.Hub;

public class MessageHub: Hub < IMessageHubClient > {
    public async Task NewComment(int userId, string message) {
        await Clients.User(userId.ToString()).NewComment(message);
    }
}