using Blog.Application.Notifications;
using Blog.Contracts;
using MassTransit;

namespace Blog.API.Notifications;

// Replaces SignalRNotificationService — PostController still depends only
// on INotificationService, the same Strategy-pattern shape as
// ICacheService/DistributedCacheService. Delivery now crosses a real
// process boundary to blog-notifications instead of an in-process
// SignalR call.
public class MessageBusNotificationService(IPublishEndpoint publishEndpoint) : INotificationService
{
    public Task NotifyNewCommentAsync(
        Guid recipientUserProfileId,
        Guid postId,
        string commentText,
        CancellationToken cancellationToken = default)
        => publishEndpoint.Publish(
            new NewCommentNotification(recipientUserProfileId, postId, commentText),
            cancellationToken);
}
