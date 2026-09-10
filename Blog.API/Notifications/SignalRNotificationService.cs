using Blog.API.Hubs;
using Blog.Application.Notifications;
using Microsoft.AspNetCore.SignalR;

namespace Blog.API.Notifications;

// The one place that knows this is SignalR — PostController depends only
// on INotificationService, the same Strategy-pattern shape as
// ICacheService/DistributedCacheService
public class SignalRNotificationService(IHubContext<MessageHub, IMessageHubClient> hubContext) : INotificationService
{
    public Task NotifyNewCommentAsync(
        Guid recipientUserProfileId,
        Guid postId,
        string commentText,
        CancellationToken cancellationToken = default)
        => hubContext.Clients.User(recipientUserProfileId.ToString()).NewComment(postId, commentText);
}
