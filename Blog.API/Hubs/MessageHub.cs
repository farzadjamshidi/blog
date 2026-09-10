using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Blog.API.Hubs;

[Authorize]
public class MessageHub : Hub<IMessageHubClient>
{
}
