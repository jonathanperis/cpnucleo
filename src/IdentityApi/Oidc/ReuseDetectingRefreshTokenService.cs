using Open.IdentityServer.Models;
using Open.IdentityServer.Services;
using Open.IdentityServer.Stores;

namespace IdentityApi.Oidc;

/// <summary>
/// One-time refresh tokens with reuse detection. Presenting a refresh token that was already
/// exchanged means two parties hold the same token (one of them stole it), so the whole session is
/// ended: its refresh tokens are revoked and API hosts stop accepting its access tokens.
/// </summary>
public sealed class ReuseDetectingRefreshTokenService(
    IRefreshTokenStore refreshTokenStore,
    IProfileService profile,
    TimeProvider clock,
    ITelemetryService telemetry,
    ILogger<DefaultRefreshTokenService> logger,
    IdentitySessions sessions)
    : DefaultRefreshTokenService(refreshTokenStore, profile, clock, telemetry, logger)
{
    protected override async Task<bool> AcceptConsumedTokenAsync(RefreshToken refreshToken)
    {
        Logger.LogWarning("A consumed refresh token was presented again; ending its session.");
        await sessions.EndAsync(refreshToken.SessionId, "refresh token reuse");
        return false;
    }
}
