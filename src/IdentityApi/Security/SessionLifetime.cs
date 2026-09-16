using System.Security.Claims;
using Domain.Common.Security;

namespace IdentityApi.Security;

public static class SessionLifetime
{
    public static bool IsRefreshable(ClaimsPrincipal user, DateTimeOffset now)
    {
        return long.TryParse(user.FindFirst(CpnucleoClaimTypes.SessionStartedAt)?.Value, out var startedAt)
            && startedAt <= now.ToUnixTimeSeconds()
            && now.ToUnixTimeSeconds() - startedAt < (long)TimeSpan.FromHours(8).TotalSeconds;
    }
}
