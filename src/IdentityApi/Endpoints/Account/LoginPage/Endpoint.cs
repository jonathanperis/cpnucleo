using Microsoft.AspNetCore.WebUtilities;
using Open.IdentityServer.Extensions;
using Open.IdentityServer.Services;

namespace IdentityApi.Endpoints.Account.LoginPage;

public class Request
{
    /// <summary>The pending authorization request the server wants the user to sign in for.</summary>
    public string? AuthRequest { get; set; }
}

/// <summary>
/// The server's login URL. It sends the browser to the sign-in page of the WebClient origin that
/// started the authorization request (its validated <c>redirect_uri</c>), so every configured
/// WebClient origin signs in on its own page. Origins outside <c>Cors:AllowedOrigins</c> never
/// receive the redirect: they get the default sign-in page, and a missing or invalid request goes
/// there with <c>error=request</c>.
/// </summary>
public class Endpoint(IIdentityServerInteractionService interaction, IConfiguration configuration) : Endpoint<Request>
{
    /// <summary>The absolute route (FastEndpoints adds the <c>api</c> prefix to <c>/account/login-page</c>).</summary>
    public const string Route = "/api/account/login-page";

    public override void Configure()
    {
        Get("/account/login-page");
        AllowAnonymous();
        Description(x => x.WithTags("Account"));

        Summary(s =>
        {
            s.Summary = "Send the browser to the WebClient sign-in page";
            s.Description = "The OpenID Connect login URL. Redirects to {origin}/login/?authRequest=... for the configured WebClient origin of the pending authorization request.";
        });
    }

    public override async Task HandleAsync(Request req, CancellationToken cancellationToken)
    {
        // The context is the re-validated authorization request: its redirect URI is registered for the client.
        if (LocalAuthorizeCallback(req.AuthRequest) is not { } returnUrl ||
            await interaction.GetAuthorizationContextAsync(returnUrl) is not { } context)
        {
            Logger.LogWarning("Sign-in page requested without a valid pending authorization request.");
            await Send.RedirectAsync(QueryHelpers.AddQueryString(OidcSettings.LoginPageUrl(configuration), "error", "request"),
                isPermanent: false, allowRemoteRedirects: true);
            return;
        }

        // The sign-in page posts the request back to this host, so it gets the absolute URL.
        var authRequest = HttpContext.GetIdentityServerHost().TrimEnd('/') + returnUrl;
        var page = OidcSettings.LoginPageUrl(configuration, context.RedirectUri);
        await Send.RedirectAsync(QueryHelpers.AddQueryString(page, "authRequest", authRequest), isPermanent: false, allowRemoteRedirects: true);
    }

    /// <summary>
    /// The pending request as a local <c>/connect/authorize/callback</c> URL. The server passes a
    /// path on this host; absolute URLs are accepted only for this host.
    /// </summary>
    private string? LocalAuthorizeCallback(string? authRequest)
    {
        if (string.IsNullOrWhiteSpace(authRequest)) return null;
        string? local = null;
        if (authRequest.StartsWith('/') && !authRequest.StartsWith("//", StringComparison.Ordinal) && !authRequest.StartsWith("/\\", StringComparison.Ordinal))
            local = authRequest;
        else if (Uri.TryCreate(authRequest, UriKind.Absolute, out var uri) && uri.Scheme is "https" or "http" &&
                 string.Equals(uri.Authority, HttpContext.Request.Host.Value, StringComparison.OrdinalIgnoreCase))
            local = uri.PathAndQuery;

        return local is not null && interaction.IsValidReturnUrl(local) &&
               local.StartsWith("/connect/authorize/callback?", StringComparison.Ordinal)
            ? local
            : null;
    }
}
