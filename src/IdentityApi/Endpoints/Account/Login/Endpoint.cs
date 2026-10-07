using Microsoft.AspNetCore.Authentication;
using Open.IdentityServer;
using Open.IdentityServer.Extensions;
using Open.IdentityServer.Services;

namespace IdentityApi.Endpoints.Account.Login;

/// <summary>
/// Completes the sign-in step of the authorization code flow. The WebClient's Astro page posts the
/// form here (a real browser form post, so the identity cookie is first-party on this host); on
/// success the browser returns to the pending authorization request, which issues the code. Errors
/// go back to the Astro page with an error code. Passwords are checked timing-safely, failures lock
/// the login, and attempts are concurrency-capped; login names are never logged.
/// </summary>
public class Endpoint(
    IApplicationDbContext dbContext,
    TimingSafePasswordCheck passwordCheck,
    LoginThrottle throttle,
    IIdentityServerInteractionService interaction,
    IdentitySessions sessions,
    IConfiguration configuration,
    TimeProvider timeProvider) : Endpoint<Request>
{
    public const string ConcurrencyPolicy = "login-concurrency";

    public override void Configure()
    {
        Post("/account/login");
        AllowAnonymous();
        AllowFormData(urlEncoded: true);
        Description(x => x.WithTags("Account"));
        // Each attempt costs an Argon2id hash (64 MiB); cap how many run at once.
        Options(x => x.RequireRateLimiting(ConcurrencyPolicy));

        Summary(s =>
        {
            s.Summary = "Sign in for a pending authorization request";
            s.Description = "Form post from the WebClient sign-in page. Redirects to the authorization request on success, or back to the sign-in page with an error code.";
        });
    }

    public override async Task HandleAsync(Request req, CancellationToken cancellationToken)
    {
        // Login CSRF defense: only the WebClient's own sign-in page may submit credentials.
        if (!SubmittedFromWebClient())
        {
            await ApiErrors.WriteAsync(HttpContext, StatusCodes.Status403Forbidden,
                "Sign-in must be submitted from the Cpnucleo sign-in page.", cancellationToken: cancellationToken);
            return;
        }

        var returnUrl = LocalReturnUrl(req.AuthRequest);
        if (returnUrl is null || !interaction.IsValidReturnUrl(returnUrl))
        {
            await BackToSignInAsync(req, "request", authRequest: null);
            return;
        }

        // Login names are personal data (and sometimes mistyped passwords): they are never logged.
        var normalizedLogin = req.Login.Trim().ToLowerInvariant();
        // Bounded before hashing so oversized inputs can't inflate Argon2 work.
        if (normalizedLogin.Length == 0 || req.Login.Length > Domain.Common.Guard.LoginMaxLength ||
            req.Password.Length == 0 || req.Password.Length > PasswordPolicy.MaximumLength)
        {
            await BackToSignInAsync(req, "invalid");
            return;
        }

        if (throttle.RetryAfter(normalizedLogin) is { } retryAfter)
        {
            Logger.LogWarning("Sign-in rejected: too many failed attempts for this login.");
            await BackToSignInAsync(req, "locked", Math.Max(1, (int)Math.Ceiling(retryAfter.TotalSeconds)));
            return;
        }

        var matches = await dbContext.Users!
            .Where(u => u.Login != null && u.Login.Trim().ToLower() == normalizedLogin)
            .Take(2).ToListAsync(cancellationToken);
        // Ambiguous legacy logins never authenticate an arbitrary account.
        var item = matches.Count == 1 ? matches[0] : null;

        // Always verify (against a dummy hash when there's no single match) so timing is uniform.
        var verified = passwordCheck.Verify(req.Password, item?.Password);
        if (item is null || !verified)
        {
            throttle.RecordFailure(normalizedLogin);
            Logger.LogWarning("Sign-in failed: unknown, ambiguous or wrong credentials.");
            await BackToSignInAsync(req, "invalid");
            return;
        }

        throttle.RecordSuccess(normalizedLogin);

        var now = timeProvider.GetUtcNow();
        var user = new IdentityServerUser(item.Id.ToString())
        {
            DisplayName = item.Login,
            AuthenticationTime = now.UtcDateTime,
            IdentityProvider = IdentityServerConstants.LocalIdentityProvider,
            AuthenticationMethods = { OidcConstants.AuthenticationMethods.Password },
            // The stamp at sign-in: a later password or login change ends this session.
            AdditionalClaims = { new Claim(CpnucleoClaimTypes.SecurityStamp, SecurityStamp.Compute(item)) }
        };
        // Absolute eight-hour session: the cookie never slides past the original sign-in.
        var properties = new AuthenticationProperties
        {
            IsPersistent = false,
            AllowRefresh = false,
            IssuedUtc = now,
            ExpiresUtc = now + OidcSettings.MaximumSessionLength
        };
        await HttpContext.SignInAsync(user, properties);
        await sessions.StartAsync(properties.GetSessionId(), item.Id, cancellationToken);

        Logger.LogInformation("Signed in user {UserId}.", item.Id);
        await Send.RedirectAsync(returnUrl, isPermanent: false, allowRemoteRedirects: false);
    }

    private bool SubmittedFromWebClient()
    {
        var origin = HttpContext.Request.Headers.Origin.ToString();
        if (string.IsNullOrEmpty(origin) && Uri.TryCreate(HttpContext.Request.Headers.Referer.ToString(), UriKind.Absolute, out var referer))
            origin = referer.GetLeftPart(UriPartial.Authority);
        return OidcSettings.WebClientOrigins(configuration).Contains(origin, StringComparer.OrdinalIgnoreCase);
    }

    /// <summary>
    /// The pending request as a local URL. The server hands the sign-in page an absolute URL on this
    /// host; anything pointing elsewhere is rejected.
    /// </summary>
    private string? LocalReturnUrl(string authRequest)
    {
        if (string.IsNullOrWhiteSpace(authRequest)) return null;
        if (authRequest.StartsWith('/') && !authRequest.StartsWith("//", StringComparison.Ordinal) && !authRequest.StartsWith("/\\", StringComparison.Ordinal))
            return authRequest;
        return Uri.TryCreate(authRequest, UriKind.Absolute, out var uri) && uri.Scheme is "https" or "http" &&
               string.Equals(uri.Authority, HttpContext.Request.Host.Value, StringComparison.OrdinalIgnoreCase)
            ? uri.PathAndQuery
            : null;
    }

    private Task BackToSignInAsync(Request req, string error, int? retryAfterSeconds = null) =>
        BackToSignInAsync(req, error, req.AuthRequest, retryAfterSeconds);

    private Task BackToSignInAsync(Request req, string error, string? authRequest, int? retryAfterSeconds = null)
    {
        var query = new Dictionary<string, string?> { ["error"] = error };
        if (!string.IsNullOrWhiteSpace(authRequest)) query["authRequest"] = authRequest;
        if (retryAfterSeconds is { } seconds) query["retryAfter"] = seconds.ToString(System.Globalization.CultureInfo.InvariantCulture);
        var url = Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString(OidcSettings.LoginPageUrl(configuration), query);
        return Send.RedirectAsync(url, isPermanent: false, allowRemoteRedirects: true);
    }
}
