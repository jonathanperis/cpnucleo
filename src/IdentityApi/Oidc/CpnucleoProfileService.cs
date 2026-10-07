using Open.IdentityServer;
using Open.IdentityServer.Extensions;
using Open.IdentityServer.Models;
using Open.IdentityServer.Services;

namespace IdentityApi.Oidc;

/// <summary>
/// Adds the Cpnucleo claims to issued tokens and decides whether a subject may still get tokens.
/// <para>
/// Claims are recomputed from the current account at every issuance, including refreshes: the login,
/// the security stamp and the admin claim (backed by <c>CPNUCLEO_ADMIN_LOGINS</c>). A subject is
/// active only while the account is active and its credentials still match the stamp captured at
/// sign-in, so a password or login change ends both the identity cookie and the refresh tokens.
/// </para>
/// </summary>
public sealed class CpnucleoProfileService(AccountDirectory accounts, IConfiguration configuration) : IProfileService
{
    public async Task GetProfileDataAsync(ProfileDataRequestContext context)
    {
        if (!Guid.TryParse(context.Subject.GetSubjectId(), out var userId)) return;
        var account = await accounts.FindActiveAsync(userId);
        if (account is null) return;

        if (!string.IsNullOrWhiteSpace(account.Login))
        {
            context.IssuedClaims.Add(new Claim(JwtClaimTypes.Name, account.Login));
            context.IssuedClaims.Add(new Claim(CpnucleoClaimTypes.Login, account.Login));
        }

        // Identity tokens only name the user; authorization data belongs in access tokens.
        if (context.Caller != IdentityServerConstants.ProfileDataCallers.ClaimsProviderAccessToken) return;

        context.IssuedClaims.Add(new Claim(CpnucleoClaimTypes.SecurityStamp, account.Stamp));
        if (AdminLogins.Contains(configuration, account.Login)) context.IssuedClaims.Add(new Claim(CpnucleoClaimTypes.Admin, "true"));
        if (await accounts.FindTenantAsync(userId) is { } tenantId)
        {
            context.IssuedClaims.Add(new Claim(CpnucleoClaimTypes.TenantId, tenantId.ToString()));
            context.IssuedClaims.Add(new Claim(CpnucleoClaimTypes.TenantSlug, tenantId.ToString()));
        }
    }

    public async Task IsActiveAsync(IsActiveContext context)
    {
        context.IsActive = false;
        if (!Guid.TryParse(context.Subject.GetSubjectId(), out var userId)) return;
        var account = await accounts.FindActiveAsync(userId);
        var stamp = context.Subject.FindFirst(CpnucleoClaimTypes.SecurityStamp)?.Value;
        context.IsActive = account is not null && string.Equals(stamp, account.Stamp, StringComparison.Ordinal);
    }
}
