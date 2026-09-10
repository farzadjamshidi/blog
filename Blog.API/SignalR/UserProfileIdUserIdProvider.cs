using Microsoft.AspNetCore.SignalR;

namespace Blog.API.SignalR;

// SignalR's default IUserIdProvider reads ClaimTypes.NameIdentifier, but
// this app's JWTs carry a custom "UserProfileId" claim instead (see
// HttpContextExtensions.GetUserProfileIdClaimValue) — without this,
// Clients.User(...) would never match any connection.
public class UserProfileIdUserIdProvider : IUserIdProvider
{
    public string? GetUserId(HubConnectionContext connection)
        => connection.User?.FindFirst("UserProfileId")?.Value;
}
