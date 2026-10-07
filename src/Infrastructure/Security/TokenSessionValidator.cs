using System.Security.Claims;
using Microsoft.Extensions.Caching.Memory;

namespace Infrastructure.Security;

/// <summary>
/// Confirms that a validated JWT still describes a live session: the account is active, its
/// credentials have not changed since the token was issued (security stamp) and an admin claim is
/// still backed by configuration. Results are cached briefly so revocation takes effect within
/// <see cref="CacheDuration"/> (<c>Auth:SessionValidationCacheSeconds</c>, 30 by default) without a
/// database round trip per request.
/// </summary>
public sealed class TokenSessionValidator(NpgsqlDataSource dataSource, IConfiguration configuration, IMemoryCache cache)
{
    public const int DefaultCacheSeconds = 30;

    public TimeSpan CacheDuration { get; } = TimeSpan.FromSeconds(
        configuration.GetValue("Auth:SessionValidationCacheSeconds", DefaultCacheSeconds));

    /// <summary>Checks that <paramref name="principal"/>'s session is still live.</summary>
    /// <returns>Null when the session is valid, otherwise the reason it is not.</returns>
    public async Task<string?> ValidateAsync(ClaimsPrincipal principal, CancellationToken cancellationToken = default)
    {
        if (!Guid.TryParse(principal.FindFirst(CpnucleoClaimTypes.Subject)?.Value, out var userId))
            return "The token has no valid subject.";

        var stamp = principal.FindFirst(CpnucleoClaimTypes.SecurityStamp)?.Value;
        if (string.IsNullOrWhiteSpace(stamp))
            return "The token was issued before session validation; sign in again.";

        var account = await cache.GetOrCreateAsync(("cpnucleo-session", userId), async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = CacheDuration;
            return await LoadAsync(userId, cancellationToken).ConfigureAwait(false);
        }).ConfigureAwait(false);

        if (account is null) return "The account is inactive or no longer exists.";
        if (!string.Equals(account.Stamp, stamp, StringComparison.Ordinal))
            return "The account credentials changed; sign in again.";
        if (principal.HasClaim(CpnucleoClaimTypes.Admin, "true") && !AdminLogins.Contains(configuration, account.Login))
            return "Administrator access was revoked.";
        return null;
    }

    private async Task<SessionAccount?> LoadAsync(Guid userId, CancellationToken cancellationToken)
    {
        await using var connection = await dataSource.OpenConnectionAsync(cancellationToken).ConfigureAwait(false);
        var row = await connection.QuerySingleOrDefaultAsync<(string? Login, string? Password)?>(new CommandDefinition(
            """SELECT "Login", "Password" FROM "Users" WHERE "Id" = @Id AND "Active" """,
            new { Id = userId }, cancellationToken: cancellationToken)).ConfigureAwait(false);
        return row is { } value ? new SessionAccount(value.Login, SecurityStamp.Compute(value.Password, value.Login)) : null;
    }

    private sealed record SessionAccount(string? Login, string Stamp);
}
