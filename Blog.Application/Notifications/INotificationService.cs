namespace Blog.Application.Notifications;

public interface INotificationService
{
    Task NotifyNewCommentAsync(
        Guid recipientUserProfileId,
        Guid postId,
        string commentText,
        CancellationToken cancellationToken = default);
}
