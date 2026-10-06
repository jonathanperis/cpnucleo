using Infrastructure.Security;
using Microsoft.IdentityModel.Tokens;

namespace IdentityApi.Security;

/// <summary>
/// Issues access tokens with the configured signing style: RS256 when a PEM key pair is configured
/// (API hosts then hold only the public key), otherwise HS256 with the shared secret.
/// </summary>
public sealed class TokenIssuer(IConfiguration configuration, TimeProvider timeProvider)
{
    public static readonly TimeSpan AccessTokenLifetime = TimeSpan.FromMinutes(30);

    public DateTimeOffset Now => timeProvider.GetUtcNow();

    /// <summary>
    /// Creates a token for <paramref name="account"/>. The security stamp and admin claim are always
    /// recomputed from the current account and configuration; <paramref name="sessionClaims"/> carries
    /// the session identity (subject, session start, tenant) that a refresh preserves.
    /// </summary>
    public string Issue(Domain.Entities.User account, IEnumerable<(string Type, string Value)> sessionClaims, DateTimeOffset expiresAt)
    {
        var asymmetric = JwtKeys.UsesAsymmetricSigning(configuration);
        return JwtBearer.CreateToken(options =>
        {
            options.SigningKey = JwtKeys.SigningKey(configuration);
            options.Issuer = JwtKeys.Issuer(configuration);
            options.Audience = JwtKeys.Audience(configuration);
            options.ExpireAt = expiresAt.UtcDateTime;
            if (asymmetric)
            {
                options.SigningStyle = TokenSigningStyle.Asymmetric;
                options.SigningAlgorithm = SecurityAlgorithms.RsaSha256;
                options.KeyIsPemEncoded = true;
            }

            foreach (var claim in sessionClaims.Distinct()) options.User.Claims.Add(claim);

            if (!string.IsNullOrWhiteSpace(account.Login))
            {
                options.User.Claims.Add((CpnucleoClaimTypes.Login, account.Login));
                options.User.Claims.Add((ClaimTypes.Name, account.Login));
            }

            options.User.Claims.Add((CpnucleoClaimTypes.SecurityStamp, SecurityStamp.Compute(account)));
            if (AdminLogins.Contains(configuration, account.Login)) options.User.Claims.Add((CpnucleoClaimTypes.Admin, "true"));
        });
    }
}
