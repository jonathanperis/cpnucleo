using Open.IdentityServer;
using Open.IdentityServer.Validation;

namespace IdentityApi.Oidc;

/// <summary>
/// Cpnucleo rules for token requests:
/// <list type="bullet">
/// <item>Refreshed access tokens keep the session id (<c>sid</c>) of the sign-in, so ending the
/// session still revokes them at the API hosts.</item>
/// <item>Client-credentials tokens act as the client's configured service account: the token gets
/// that account's subject and security stamp, and the profile service adds its login and admin
/// claim. API hosts then apply the same session rules as for people, so deactivating the account or
/// changing its credentials revokes the client too. Without an active account no token is issued.</item>
/// </list>
/// </summary>
public sealed class CpnucleoTokenRequestValidator(AccountDirectory accounts, TimeProvider timeProvider) : ICustomTokenRequestValidator
{
    public async Task ValidateAsync(CustomTokenRequestValidationContext context)
    {
        var request = context.Result.ValidatedRequest;
        if (request.GrantType == OidcConstants.GrantTypes.RefreshToken && string.IsNullOrEmpty(request.SessionId))
        {
            request.SessionId = request.RefreshToken?.SessionId;
            return;
        }

        if (request.GrantType != OidcConstants.GrantTypes.ClientCredentials) return;

        var login = request.Client.Properties.TryGetValue(OidcSettings.ServiceAccountProperty, out var value) ? value : null;
        var account = string.IsNullOrWhiteSpace(login) ? null : await accounts.FindActiveByLoginAsync(login);
        if (account is null)
        {
            context.Result.IsError = true;
            context.Result.Error = OidcConstants.TokenErrors.InvalidClient;
            context.Result.ErrorDescription = "The client's service account is not available.";
            return;
        }

        request.Subject = new IdentityServerUser(account.Id.ToString())
        {
            AuthenticationTime = timeProvider.GetUtcNow().UtcDateTime,
            IdentityProvider = IdentityServerConstants.LocalIdentityProvider,
            AdditionalClaims = { new Claim(CpnucleoClaimTypes.SecurityStamp, account.Stamp) }
        }.CreatePrincipal();
    }
}
