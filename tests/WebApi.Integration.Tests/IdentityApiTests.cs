using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Dapper;
using Grpc.Core;
using GrpcServer.Contracts.Commands.Project;
using Infrastructure.Common.Security;

namespace WebApi.Integration.Tests;

/// <summary>
/// The real IdentityApi OpenID Connect server on PostgreSQL, driven like a browser: authorization
/// code with PKCE, one-time refresh tokens, revocation, sign-out and service clients, end to end
/// against both API hosts.
/// </summary>
[Collection("Database")]
public class IdentityApiTests(WebAppFixture app)
{
    private const string Password = "Integration@123";
    private static CancellationToken Cancellation => TestContext.Current.CancellationToken;

    [Fact]
    public async Task SigningIn_IssuesAnAccessTokenForTheRestApiOnly()
    {
        var login = await CreateUserAsync();
        using var browser = app.CreateIdentityClient();

        var tokens = await OidcBrowser.SignInAsync(browser, login, Password);

        (await CallApiAsync(tokens.AccessToken)).ShouldBe(HttpStatusCode.OK);
        var token = new JwtSecurityTokenHandler().ReadJwtToken(tokens.AccessToken);
        token.Header.Typ.ShouldBe("at+jwt");
        token.Header.Alg.ShouldBe("RS256");
        token.Header.Kid.ShouldBe(WebAppFixture.SigningKeyId);
        token.Issuer.ShouldBe(WebAppFixture.Issuer);
        token.Audiences.ShouldBe([WebAppFixture.Audience]);
        token.Claims.ShouldContain(claim => claim.Type == "sid");
        token.Claims.ShouldContain(claim => claim.Type == "cpnucleo:login" && claim.Value == login);
        token.Claims.ShouldContain(claim => claim.Type == "cpnucleo:security_stamp");
        token.Claims.ShouldNotContain(claim => claim.Type == "cpnucleo:admin");

        var command = new ListProjectsCommand { Pagination = new PaginationParams() };
        (await Should.ThrowAsync<RpcException>(() => command.RemoteExecuteAsync(WebAppFixture.GrpcOptions(tokens.AccessToken))))
            .StatusCode.ShouldBe(StatusCode.Unauthenticated, "a token for WebApi has the wrong audience for GrpcServer");
    }

    [Fact]
    public async Task RefreshTokens_AreOneTimeUse_AndReplayingOneEndsTheSession()
    {
        var login = await CreateUserAsync();
        using var browser = app.CreateIdentityClient();
        var first = await OidcBrowser.SignInAsync(browser, login, Password);

        var second = await OidcBrowser.ReadTokensAsync(await OidcBrowser.RefreshAsync(browser, first.RefreshToken));
        second.RefreshToken.ShouldNotBe(first.RefreshToken);
        (await CallApiAsync(second.AccessToken)).ShouldBe(HttpStatusCode.OK);

        // Someone replays the consumed token: the whole session ends, including the newest tokens.
        var replay = await OidcBrowser.RefreshAsync(browser, first.RefreshToken);
        replay.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        (await OidcBrowser.ErrorAsync(replay)).ShouldBe("invalid_grant");
        (await OidcBrowser.ErrorAsync(await OidcBrowser.RefreshAsync(browser, second.RefreshToken))).ShouldBe("invalid_grant");
        await WaitForSessionCacheAsync();
        (await CallApiAsync(second.AccessToken)).ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task SigningOut_EndsTheSessionForEveryToken()
    {
        var login = await CreateUserAsync();
        using var browser = app.CreateIdentityClient();
        var tokens = await OidcBrowser.SignInAsync(browser, login, Password);
        (await CallApiAsync(tokens.AccessToken)).ShouldBe(HttpStatusCode.OK, "the session is now cached");

        var landing = await OidcBrowser.SignOutAsync(browser, tokens.IdToken);

        landing.ShouldBe(new Uri($"{OidcBrowser.PostLogoutRedirectUri}?state=bye"));
        (await OidcBrowser.ErrorAsync(await OidcBrowser.RefreshAsync(browser, tokens.RefreshToken))).ShouldBe("invalid_grant");
        await WaitForSessionCacheAsync();
        (await CallApiAsync(tokens.AccessToken)).ShouldBe(HttpStatusCode.Unauthorized);
        // The identity cookie is gone too: the next authorization asks for credentials again.
        (await OidcBrowser.AuthorizeAsync(browser, OidcBrowser.Pkce.Create())).GetLeftPart(UriPartial.Path)
            .ShouldBe($"{WebAppFixture.WebOrigin}/login/");
    }

    [Fact]
    public async Task TheRevocationEndpoint_RevokesARefreshToken()
    {
        var login = await CreateUserAsync();
        using var browser = app.CreateIdentityClient();
        var tokens = await OidcBrowser.SignInAsync(browser, login, Password);

        using var form = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["token"] = tokens.RefreshToken,
            ["token_type_hint"] = "refresh_token",
            ["client_id"] = OidcBrowser.ClientId
        });
        var revoked = await browser.PostAsync("/connect/revocation", form, Cancellation);

        revoked.StatusCode.ShouldBe(HttpStatusCode.OK);
        (await OidcBrowser.ErrorAsync(await OidcBrowser.RefreshAsync(browser, tokens.RefreshToken))).ShouldBe("invalid_grant");
    }

    [Fact]
    public async Task AnExistingSession_SignsInWithoutAskingAgain()
    {
        var login = await CreateUserAsync();
        using var browser = app.CreateIdentityClient();
        await OidcBrowser.SignInAsync(browser, login, Password);

        var pkce = OidcBrowser.Pkce.Create();
        var callback = await OidcBrowser.FollowToCallbackAsync(browser, new Uri(OidcBrowser.AuthorizeUrl(pkce), UriKind.Relative));
        var tokens = await OidcBrowser.RedeemAsync(browser, OidcBrowser.Query(callback, "code"), pkce);

        (await CallApiAsync(tokens.AccessToken)).ShouldBe(HttpStatusCode.OK);
    }

    [Fact]
    public async Task ChangingThePassword_EndsExistingSessions()
    {
        var login = await CreateUserAsync();
        using var browser = app.CreateIdentityClient();
        var tokens = await OidcBrowser.SignInAsync(browser, login, Password);

        await using (var connection = app.CreateConnection())
        {
            var newHash = new Argon2PasswordHasher().Hash("Changed@1234").Hash;
            await connection.ExecuteAsync("""UPDATE "Users" SET "Password" = @newHash WHERE "Login" = @login""", new { newHash, login });
        }

        (await OidcBrowser.ErrorAsync(await OidcBrowser.RefreshAsync(browser, tokens.RefreshToken))).ShouldBe("invalid_grant");
        await WaitForSessionCacheAsync();
        (await CallApiAsync(tokens.AccessToken)).ShouldBe(HttpStatusCode.Unauthorized);
        (await OidcBrowser.AuthorizeAsync(browser, OidcBrowser.Pkce.Create())).GetLeftPart(UriPartial.Path)
            .ShouldBe($"{WebAppFixture.WebOrigin}/login/", "the identity cookie no longer matches the credentials");
    }

    [Fact]
    public async Task UnknownAndWrongCredentials_LookTheSame_AndKeepThePendingRequest()
    {
        var login = await CreateUserAsync();
        using var browser = app.CreateIdentityClient();
        var authRequest = OidcBrowser.Query(await OidcBrowser.AuthorizeAsync(browser, OidcBrowser.Pkce.Create()), "authRequest");

        var wrong = await OidcBrowser.PostSignInAsync(browser, login, "Wrong@12345", authRequest);
        var unknown = await OidcBrowser.PostSignInAsync(browser, $"ghost-{Guid.NewGuid():N}", "Wrong@12345", authRequest);

        wrong.StatusCode.ShouldBe(HttpStatusCode.Found);
        wrong.Headers.Location.ShouldBe(unknown.Headers.Location);
        OidcBrowser.Query(wrong.Headers.Location!, "error").ShouldBe("invalid");
        OidcBrowser.Query(wrong.Headers.Location!, "authRequest").ShouldBe(authRequest);
    }

    [Fact]
    public async Task AmbiguousLegacyLogins_NeverAuthenticate()
    {
        var login = $"twin-{Guid.NewGuid():N}";
        var hash = new Argon2PasswordHasher().Hash(Password).Hash;
        await using (var connection = app.CreateConnection())
        {
            // Legacy duplicates predate the login integrity trigger; recreate them with it disabled.
            await connection.OpenAsync(Cancellation);
            await using var transaction = await connection.BeginTransactionAsync(Cancellation);
            await connection.ExecuteAsync("""ALTER TABLE "Users" DISABLE TRIGGER "Users_LoginIntegrity" """, transaction: transaction);
            foreach (var variant in new[] { login, $" {login.ToUpperInvariant()} " })
                await connection.ExecuteAsync("""
                    INSERT INTO "Users" ("Id", "Name", "Login", "Password", "Salt", "CreatedAt", "Active")
                    VALUES (@Id, 'Twin', @variant, @hash, '', now(), true)
                    """, new { Id = Guid.CreateVersion7(), variant, hash }, transaction);
            await connection.ExecuteAsync("""ALTER TABLE "Users" ENABLE TRIGGER "Users_LoginIntegrity" """, transaction: transaction);
            await transaction.CommitAsync(Cancellation);
        }

        using var browser = app.CreateIdentityClient();
        var authRequest = OidcBrowser.Query(await OidcBrowser.AuthorizeAsync(browser, OidcBrowser.Pkce.Create()), "authRequest");
        var response = await OidcBrowser.PostSignInAsync(browser, login, Password, authRequest);

        OidcBrowser.Query(response.Headers.Location!, "error").ShouldBe("invalid");
    }

    [Fact]
    public async Task RepeatedFailures_LockTheLogin()
    {
        var login = await CreateUserAsync();
        using var browser = app.CreateIdentityClient();
        var authRequest = OidcBrowser.Query(await OidcBrowser.AuthorizeAsync(browser, OidcBrowser.Pkce.Create()), "authRequest");

        for (var attempt = 0; attempt < IdentityApi.Security.LoginThrottle.MaximumFailures; attempt++)
            OidcBrowser.Query((await OidcBrowser.PostSignInAsync(browser, login, "Wrong@12345", authRequest)).Headers.Location!, "error").ShouldBe("invalid");

        var locked = (await OidcBrowser.PostSignInAsync(browser, login, Password, authRequest)).Headers.Location!;
        OidcBrowser.Query(locked, "error").ShouldBe("locked");
        int.Parse(OidcBrowser.Query(locked, "retryAfter")).ShouldBeGreaterThan(0);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("https://attacker.test")]
    public async Task CredentialsPostedFromAnotherOrigin_AreRefused(string? origin)
    {
        var login = await CreateUserAsync();
        using var browser = app.CreateIdentityClient();
        var authRequest = OidcBrowser.Query(await OidcBrowser.AuthorizeAsync(browser, OidcBrowser.Pkce.Create()), "authRequest");

        var response = await OidcBrowser.PostSignInAsync(browser, login, Password, authRequest, origin);

        response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
        (await response.Content.ReadFromJsonAsync<JsonElement>(Cancellation)).GetProperty("statusCode").GetInt32().ShouldBe(403);
        response.Headers.GetValues("X-Frame-Options").Single().ShouldBe("DENY");
    }

    [Fact]
    public async Task SignIn_OnlyReturnsToAPendingAuthorizationRequest()
    {
        var login = await CreateUserAsync();
        using var browser = app.CreateIdentityClient();

        var response = await OidcBrowser.PostSignInAsync(browser, login, Password, "https://attacker.test/connect/authorize/callback?x=1");

        response.StatusCode.ShouldBe(HttpStatusCode.Found);
        response.Headers.Location!.GetLeftPart(UriPartial.Path).ShouldBe($"{WebAppFixture.WebOrigin}/login/");
        OidcBrowser.Query(response.Headers.Location!, "error").ShouldBe("request");
    }

    [Theory]
    [InlineData(false, "")]
    [InlineData(true, "https://attacker.test/callback")]
    public async Task AuthorizationRequests_WithoutPkceOrWithAForeignRedirect_NeverReachTheClient(bool withPkce, string redirectUri)
    {
        using var browser = app.CreateIdentityClient();

        var response = await browser.GetAsync(OidcBrowser.AuthorizeUrl(withPkce ? OidcBrowser.Pkce.Create() : null, redirectUri), Cancellation);

        // No code and no redirect to the requested URI: the sign-in page explains the error instead.
        var errorPage = response.Headers.Location!;
        errorPage.GetLeftPart(UriPartial.Path).ShouldBe($"{WebAppFixture.WebOrigin}/login/");
        var error = await browser.GetAsync($"/api/account/error?errorId={Uri.EscapeDataString(OidcBrowser.Query(errorPage, "errorId"))}", Cancellation);
        error.StatusCode.ShouldBe(HttpStatusCode.OK);
        (await error.Content.ReadFromJsonAsync<JsonElement>(Cancellation)).GetProperty("error").GetString().ShouldNotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task Discovery_PublishesTheProtocolEndpointsAndTheSigningKeys()
    {
        using var browser = app.CreateIdentityClient();

        var discovery = await browser.GetFromJsonAsync<JsonElement>("/.well-known/openid-configuration", Cancellation);
        discovery.GetProperty("issuer").GetString().ShouldBe(WebAppFixture.Issuer);
        foreach (var endpoint in new[] { "authorization_endpoint", "token_endpoint", "revocation_endpoint", "introspection_endpoint", "end_session_endpoint", "jwks_uri" })
            discovery.TryGetProperty(endpoint, out _).ShouldBeTrue(endpoint);
        discovery.TryGetProperty("device_authorization_endpoint", out _).ShouldBeFalse("the device flow is disabled");

        var keys = await browser.GetFromJsonAsync<JsonElement>("/.well-known/openid-configuration/jwks", Cancellation);
        keys.GetProperty("keys").EnumerateArray().Select(key => key.GetProperty("kid").GetString()).ShouldContain(WebAppFixture.SigningKeyId);
        keys.GetProperty("keys").EnumerateArray().Where(key => key.TryGetProperty("d", out var _)).ShouldBeEmpty("only public key parameters are published");
    }

    [Fact]
    public async Task TheKeyRing_ReplacesKeysEncryptedUnderAnotherSecret()
    {
        var unreadable = $"unreadable-{Guid.NewGuid():N}";
        await using (var connection = app.CreateConnection())
        {
            // A key left behind by a previous key-encryption secret, active right now.
            await connection.ExecuteAsync("""
                INSERT INTO "IdentitySigningKeys" ("Id", "Algorithm", "ProtectedKey", "CreatedAt", "ActivatesAt", "RetiresAt")
                VALUES (@unreadable, 'RS256', @garbage, now(), now(), now() + interval '30 days')
                """, new { unreadable, garbage = new byte[64] });
        }

        // An empty pinned key makes this host use the stored key ring.
        var (api, identity) = app.CreateHosts(new Dictionary<string, string?> { ["Jwt:SigningPrivateKey"] = "" });
        await using (api)
        await using (identity)
        {
            using var client = identity.CreateClient();
            var keys = await client.GetFromJsonAsync<JsonElement>("/.well-known/openid-configuration/jwks", Cancellation);
            var published = keys.GetProperty("keys").EnumerateArray().Select(key => key.GetProperty("kid").GetString()).ToList();

            published.ShouldNotBeEmpty("a replacement key is created");
            published.ShouldNotContain(unreadable);
            await using var connection = app.CreateConnection();
            (await connection.ExecuteScalarAsync<int>("""SELECT count(*) FROM "IdentitySigningKeys" WHERE "Id" = @unreadable""", new { unreadable })).ShouldBe(0);
        }
    }

    [Fact]
    public async Task ServiceClients_ActAsTheirServiceAccount_OnTheApiTheyRequested()
    {
        const string secret = "integration-service-client-secret-0123456789";
        var (api, identity) = app.CreateHosts(new Dictionary<string, string?>
        {
            ["Identity:ServiceClients:lab-tool:Secret"] = secret,
            ["Identity:ServiceClients:lab-tool:Login"] = WebAppFixture.Member.Login,
            ["Identity:ServiceClients:ghost-tool:Secret"] = secret,
            ["Identity:ServiceClients:ghost-tool:Login"] = "nobody@integration.test"
        });
        await using (api)
        await using (identity)
        {
            using var client = identity.CreateClient();
            var response = await ClientCredentialsAsync(client, "lab-tool", secret, "cpnucleo.grpc");
            using var json = JsonDocument.Parse(await response.Content.ReadAsStringAsync(Cancellation));
            response.StatusCode.ShouldBe(HttpStatusCode.OK, json.RootElement.ToString());
            var accessToken = json.RootElement.GetProperty("access_token").GetString()!;

            var token = new JwtSecurityTokenHandler().ReadJwtToken(accessToken);
            token.Subject.ShouldBe(WebAppFixture.Member.Id.ToString());
            token.Audiences.ShouldBe([WebAppFixture.GrpcAudience]);
            token.Claims.ShouldNotContain(claim => claim.Type == "sid");

            (await new ListProjectsCommand { Pagination = new PaginationParams() }.RemoteExecuteAsync(WebAppFixture.GrpcOptions(accessToken)))
                .ShouldNotBeNull();
            (await CallApiAsync(accessToken)).ShouldBe(HttpStatusCode.Unauthorized, "the token was requested for GrpcServer only");

            var ghost = await ClientCredentialsAsync(client, "ghost-tool", secret, "cpnucleo.grpc");
            ghost.StatusCode.ShouldBe(HttpStatusCode.BadRequest, "a client without an active service account gets no token");
            (await ClientCredentialsAsync(client, "lab-tool", "a-wrong-secret-of-a-plausible-length-000", "cpnucleo.grpc"))
                .StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        }
    }

    [Fact]
    public async Task PerAddressRateLimit_RejectsWithRetryAfterAndTheErrorEnvelope()
    {
        await using var limited = app.CreateIdentityFactory(disableRateLimiting: false);
        using var client = limited.CreateClient();
        using var timeout = CancellationTokenSource.CreateLinkedTokenSource(Cancellation);
        timeout.CancelAfter(TimeSpan.FromSeconds(30));

        // 60 permits per window plus a queue of 5: a burst of 70 is partly rejected immediately.
        var pending = Enumerable.Range(0, 70)
            .Select(_ => client.GetAsync("/.well-known/openid-configuration", timeout.Token))
            .ToList();
        HttpResponseMessage? rejected = null;
        while (pending.Count > 0 && rejected is null)
        {
            var completed = await Task.WhenAny(pending);
            pending.Remove(completed);
            var response = await completed;
            if (response.StatusCode == HttpStatusCode.TooManyRequests) rejected = response;
        }

        rejected.ShouldNotBeNull();
        rejected.Headers.RetryAfter.ShouldNotBeNull();
        (await rejected.Content.ReadFromJsonAsync<JsonElement>(Cancellation)).GetProperty("statusCode").GetInt32().ShouldBe(429);
        await timeout.CancelAsync();
    }

    [Fact]
    public async Task WithoutAnAdminList_TheSeededDemoAccountAdministersBothApis()
    {
        await using (var connection = app.CreateConnection())
        {
            await connection.ExecuteAsync("""
                INSERT INTO "Users" ("Id", "Name", "Login", "Password", "Salt", "CreatedAt", "Active")
                VALUES (@Id, 'Cpnucleo Demo', @login, @hash, '', now(), true)
                ON CONFLICT DO NOTHING
                """, new { Id = Guid.CreateVersion7(), login = Infrastructure.Security.AdminLogins.DefaultLogin, hash = new Argon2PasswordHasher().Hash(Password).Hash });
        }

        var (api, identity) = app.CreateHosts(new Dictionary<string, string?> { [Infrastructure.Security.AdminLogins.ConfigurationKey] = "" });
        await using (api)
        await using (identity)
        {
            using var browser = WebAppFixture.CreateBrowserClient(identity);
            var tokens = await OidcBrowser.SignInAsync(browser, Infrastructure.Security.AdminLogins.DefaultLogin, Password);
            new JwtSecurityTokenHandler().ReadJwtToken(tokens.AccessToken).Claims
                .ShouldContain(claim => claim.Type == "cpnucleo:admin" && claim.Value == "true");

            using var apiClient = api.CreateClient();
            apiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokens.AccessToken);
            (await apiClient.GetAsync("/api/users?pageSize=1", Cancellation)).StatusCode.ShouldBe(HttpStatusCode.OK, "user administration is admin-only");
            (await apiClient.PostAsJsonAsync("/api/workflow", new { id = Guid.NewGuid(), name = "Demo admin workflow", order = 1 }, Cancellation))
                .StatusCode.ShouldBe(HttpStatusCode.OK, "catalog writes are admin-only");
        }
    }

    private async Task<HttpStatusCode> CallApiAsync(string accessToken)
    {
        using var api = app.CreateClient();
        api.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        return (await api.GetAsync("/api/organizations?pageSize=1", Cancellation)).StatusCode;
    }

    private static async Task<HttpResponseMessage> ClientCredentialsAsync(HttpClient client, string clientId, string secret, string scope)
    {
        using var request = new HttpRequestMessage(HttpMethod.Post, "/connect/token")
        {
            Content = new FormUrlEncodedContent(new Dictionary<string, string> { ["grant_type"] = "client_credentials", ["scope"] = scope })
        };
        request.Headers.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.UTF8.GetBytes($"{clientId}:{secret}")));
        return await client.SendAsync(request, Cancellation);
    }

    // The fixture's session validation cache lasts one second (production: 30).
    private static Task WaitForSessionCacheAsync() => Task.Delay(TimeSpan.FromSeconds(1.5), Cancellation);

    private async Task<string> CreateUserAsync()
    {
        var login = $"identity-{Guid.NewGuid():N}@integration.test";
        await using var connection = app.CreateConnection();
        await connection.ExecuteAsync("""
            INSERT INTO "Users" ("Id", "Name", "Login", "Password", "Salt", "CreatedAt", "Active")
            VALUES (@Id, 'Identity', @login, @hash, '', now(), true)
            """, new { Id = Guid.CreateVersion7(), login, hash = new Argon2PasswordHasher().Hash(Password).Hash });
        return login;
    }
}
