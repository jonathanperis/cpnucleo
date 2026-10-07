using Microsoft.AspNetCore.Authentication;
using Open.IdentityServer.Services;

namespace IdentityApi.Endpoints.Account.Logout;

public class Request
{
    /// <summary>The logout context created by the end-session endpoint.</summary>
    public string? LogoutId { get; set; }
}

/// <summary>
/// The logout step of the end-session endpoint (<c>/connect/endsession</c> redirects here). Ends the
/// server-side session (revoking its refresh tokens; API hosts then reject its access tokens),
/// removes the identity cookie and returns to the client's registered post-logout URL. No prompt is
/// shown: signing out is always safe to perform.
/// </summary>
public class Endpoint(
    IIdentityServerInteractionService interaction,
    IUserSession userSession,
    IdentitySessions sessions,
    IConfiguration configuration) : Endpoint<Request>
{
    public override void Configure()
    {
        Get("/account/logout");
        AllowAnonymous();
        Description(x => x.WithTags("Account"));
        Summary(s =>
        {
            s.Summary = "Sign out of the identity session";
            s.Description = "Reached through the OpenID Connect end-session endpoint. Ends the session and redirects to the post-logout URL.";
        });
    }

    public override async Task HandleAsync(Request req, CancellationToken cancellationToken)
    {
        var logout = await interaction.GetLogoutContextAsync(req.LogoutId);
        var sessionId = await userSession.GetSessionIdAsync() ?? logout?.SessionId;
        await sessions.EndAsync(sessionId, "sign-out", cancellationToken);
        if (User.Identity?.IsAuthenticated == true) await HttpContext.SignOutAsync();

        await Send.RedirectAsync(logout?.PostLogoutRedirectUri ?? OidcSettings.LoginPageUrl(configuration),
            isPermanent: false, allowRemoteRedirects: true);
    }
}
