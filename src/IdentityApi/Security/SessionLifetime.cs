using System.Security.Claims;
using Domain.Common.Security;

namespace IdentityApi.Security;

public static class SessionLifetime
{
    public static readonly TimeSpan MaximumSessionLength = TimeSpan.FromHours(8);

    public static bool IsRefreshable(ClaimsPrincipal user, DateTimeOffset now)
    {
        return long.TryParse(user.FindFirst(CpnucleoClaimTypes.SessionStartedAt)?.Value, out var startedAt)
            && startedAt <= now.ToUnixTimeSeconds()
            && now.ToUnixTimeSeconds() - startedAt < (long)MaximumSessionLength.TotalSeconds;
    }

    /// <summary>When the session that issued <paramref name="user"/>'s token must end. Call after <see cref="IsRefreshable"/>.</summary>
    public static DateTimeOffset EndsAt(ClaimsPrincipal user) =>
        DateTimeOffset.FromUnixTimeSeconds(long.Parse(user.FindFirst(CpnucleoClaimTypes.SessionStartedAt)!.Value,
            System.Globalization.CultureInfo.InvariantCulture)) + MaximumSessionLength;
}
