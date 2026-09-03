using System.Security.Claims;

namespace Blog.Api.Extensions;

public static class HttpContextExtensions
{
    public static Guid GetUserProfileIdClaimValue(this HttpContext context)
    {
        return GetGuidClaimValue("UserProfileId", context);
    }

    public static Guid GetIdentityIdClaimValue(this HttpContext context)
    {
        return GetGuidClaimValue("IdentityId", context);
    }

    private static Guid GetGuidClaimValue(string key, HttpContext context)
    {
        var identity = context.User.Identity as ClaimsIdentity;
        var claimValue = identity?.FindFirst(key)?.Value
            ?? throw new UnauthorizedAccessException($"Missing '{key}' claim");

        ReadOnlySpan<char> span = claimValue.AsSpan().Trim();
        return Guid.Parse(span);
    }
}