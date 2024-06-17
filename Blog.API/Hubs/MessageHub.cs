using Microsoft.AspNetCore.SignalR;

namespace Blog.API.Hubs;

public class MessageHub: Hub < IMessageHubClient > {
    public async Task NewComment(int userId, string message) {
        await Clients.All.NewComment(message);
    }
}