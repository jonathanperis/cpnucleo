using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.WebUtilities;

namespace WebApi.Integration.Tests.Hosts;

/// <summary>
/// Drives the WebClient's OpenID Connect flow against the IdentityApi test server the way a browser
/// does: authorize, the sign-in form post, the callback with the code, and token requests.
/// </summary>
public static class OidcBrowser
{
    public const string ClientId = "cpnucleo-webclient";
    public const string Scopes = "openid profile cpnucleo.api offline_access";
    public static readonly string RedirectUri = $"{WebAppFixture.WebOrigin}/signin-callback/";
    public static readonly string PostLogoutRedirectUri = $"{WebAppFixture.WebOrigin}/login/";

    public sealed record Tokens(string AccessToken, string RefreshToken, string IdToken);

    public sealed record Pkce(string Verifier, string Challenge)
    {
        public static Pkce Create()
        {
            var verifier = Base64Url(RandomNumberGenerator.GetBytes(32));
            return new Pkce(verifier, Base64Url(SHA256.HashData(Encoding.ASCII.GetBytes(verifier))));
        }
    }

    private static CancellationToken Cancellation => TestContext.Current.CancellationToken;

    public static string AuthorizeUrl(Pkce? pkce, string redirectUri = "", string state = "state-1", string nonce = "nonce-1")
    {
        var query = new Dictionary<string, string?>
        {
            ["client_id"] = ClientId,
            ["response_type"] = "code",
            ["scope"] = Scopes,
            ["redirect_uri"] = redirectUri is "" ? RedirectUri : redirectUri,
            ["state"] = state,
            ["nonce"] = nonce
        };
        if (pkce is not null)
        {
            query["code_challenge"] = pkce.Challenge;
            query["code_challenge_method"] = "S256";
        }

        return QueryHelpers.AddQueryString("/connect/authorize", query);
    }

    /// <summary>
    /// Starts an authorization request; without a session the server sends the browser to the sign-in
    /// page, through its login step that picks the page of the WebClient origin that asked.
    /// </summary>
    public static async Task<Uri> AuthorizeAsync(HttpClient browser, Pkce pkce, string redirectUri = "")
    {
        var response = await browser.GetAsync(AuthorizeUrl(pkce, redirectUri), Cancellation);
        response.StatusCode.ShouldBe(HttpStatusCode.Found);
        var location = response.Headers.Location!;
        if (!location.OriginalString.Contains("/api/account/login-page", StringComparison.Ordinal)) return location;

        var loginStep = await browser.GetAsync(location, Cancellation);
        loginStep.StatusCode.ShouldBe(HttpStatusCode.Found, await loginStep.Content.ReadAsStringAsync(Cancellation));
        return loginStep.Headers.Location!;
    }

    public static string Query(Uri uri, string name) => QueryHelpers.ParseQuery(uri.Query).TryGetValue(name, out var value) ? value.ToString() : "";

    public static async Task<HttpResponseMessage> PostSignInAsync(HttpClient browser, string login, string password, string authRequest,
        string? origin = WebAppFixture.WebOrigin)
    {
        using var request = new HttpRequestMessage(HttpMethod.Post, "/api/account/login")
        {
            Content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["login"] = login,
                ["password"] = password,
                ["authRequest"] = authRequest
            })
        };
        if (origin is not null) request.Headers.Add("Origin", origin);
        return await browser.SendAsync(request, Cancellation);
    }

    /// <summary>The whole sign-in: authorize, sign-in form, callback and code redemption.</summary>
    public static async Task<Tokens> SignInAsync(HttpClient browser, string login, string password)
    {
        var pkce = Pkce.Create();
        var signInPage = await AuthorizeAsync(browser, pkce);
        signInPage.GetLeftPart(UriPartial.Path).ShouldBe($"{WebAppFixture.WebOrigin}/login/");

        var signedIn = await PostSignInAsync(browser, login, password, Query(signInPage, "authRequest"));
        signedIn.StatusCode.ShouldBe(HttpStatusCode.Found, await signedIn.Content.ReadAsStringAsync(Cancellation));
        var callback = await FollowToCallbackAsync(browser, signedIn.Headers.Location!);
        return await RedeemAsync(browser, Query(callback, "code"), pkce);
    }

    /// <summary>Follows the server's redirects until the browser lands on the WebClient callback.</summary>
    public static async Task<Uri> FollowToCallbackAsync(HttpClient browser, Uri location)
    {
        for (var hop = 0; hop < 5; hop++)
        {
            if (location.IsAbsoluteUri && location.GetLeftPart(UriPartial.Path) == RedirectUri) return location;
            var response = await browser.GetAsync(location, Cancellation);
            response.StatusCode.ShouldBe(HttpStatusCode.Found, await response.Content.ReadAsStringAsync(Cancellation));
            location = response.Headers.Location!;
        }

        throw new InvalidOperationException("The authorization request did not reach the callback.");
    }

    public static async Task<Tokens> RedeemAsync(HttpClient browser, string code, Pkce pkce)
    {
        var response = await TokenRequestAsync(browser, new Dictionary<string, string>
        {
            ["grant_type"] = "authorization_code",
            ["code"] = code,
            ["redirect_uri"] = RedirectUri,
            ["code_verifier"] = pkce.Verifier
        });
        return await ReadTokensAsync(response);
    }

    public static Task<HttpResponseMessage> RefreshAsync(HttpClient browser, string refreshToken) =>
        TokenRequestAsync(browser, new Dictionary<string, string> { ["grant_type"] = "refresh_token", ["refresh_token"] = refreshToken });

    public static async Task<Tokens> ReadTokensAsync(HttpResponseMessage response)
    {
        var body = await response.Content.ReadAsStringAsync(Cancellation);
        response.StatusCode.ShouldBe(HttpStatusCode.OK, body);
        using var json = JsonDocument.Parse(body);
        return new Tokens(
            json.RootElement.GetProperty("access_token").GetString()!,
            json.RootElement.GetProperty("refresh_token").GetString()!,
            json.RootElement.TryGetProperty("id_token", out var idToken) ? idToken.GetString()! : "");
    }

    public static async Task<string> ErrorAsync(HttpResponseMessage response)
    {
        using var json = JsonDocument.Parse(await response.Content.ReadAsStringAsync(Cancellation));
        return json.RootElement.GetProperty("error").GetString()!;
    }

    /// <summary>Signs out through the end-session endpoint; returns where the browser ends up.</summary>
    public static async Task<Uri> SignOutAsync(HttpClient browser, string idToken, string state = "bye")
    {
        var endSession = await browser.GetAsync(QueryHelpers.AddQueryString("/connect/endsession", new Dictionary<string, string?>
        {
            ["id_token_hint"] = idToken,
            ["post_logout_redirect_uri"] = PostLogoutRedirectUri,
            ["state"] = state
        }), Cancellation);
        endSession.StatusCode.ShouldBe(HttpStatusCode.Found);
        var logout = await browser.GetAsync(endSession.Headers.Location!, Cancellation);
        logout.StatusCode.ShouldBe(HttpStatusCode.Found, await logout.Content.ReadAsStringAsync(Cancellation));
        return logout.Headers.Location!;
    }

    private static async Task<HttpResponseMessage> TokenRequestAsync(HttpClient browser, Dictionary<string, string> form)
    {
        form["client_id"] = ClientId;
        using var content = new FormUrlEncodedContent(form);
        return await browser.PostAsync("/connect/token", content, Cancellation);
    }

    private static string Base64Url(byte[] bytes) => Convert.ToBase64String(bytes).TrimEnd('=').Replace('+', '-').Replace('/', '_');
}
