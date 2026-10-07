using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Dapper;
using Infrastructure.Common.Security;

namespace WebApi.Integration.Tests;

/// <summary>The real IdentityApi host on PostgreSQL: login, refresh and their protections end to end.</summary>
[Collection("Database")]
public class IdentityApiTests(WebAppFixture app)
{
    private const string Password = "Integration@123";
    private static CancellationToken Cancellation => TestContext.Current.CancellationToken;

    [Fact]
    public async Task IssuedTokens_WorkAgainstTheApis_AndRefreshKeepsTheSession()
    {
        var login = await CreateUserAsync();
        using var identity = app.CreateIdentityClient();

        var token = await LoginAsync(identity, login);
        using var api = app.CreateClient();
        api.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        (await api.GetAsync("/api/organizations?pageSize=1", Cancellation)).StatusCode.ShouldBe(HttpStatusCode.OK);

        identity.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var refreshed = await identity.PostAsync("/api/refresh", null, Cancellation);
        refreshed.StatusCode.ShouldBe(HttpStatusCode.OK);
        var refreshedToken = (await refreshed.Content.ReadFromJsonAsync<JsonElement>(Cancellation)).GetProperty("token").GetString();
        api.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", refreshedToken);
        (await api.GetAsync("/api/organizations?pageSize=1", Cancellation)).StatusCode.ShouldBe(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Refresh_RequiresABearerToken()
    {
        using var identity = app.CreateIdentityClient();
        var response = await identity.PostAsync("/api/refresh", null, Cancellation);

        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
        (await response.Content.ReadFromJsonAsync<JsonElement>(Cancellation)).GetProperty("statusCode").GetInt32().ShouldBe(401);
    }

    [Fact]
    public async Task ChangingThePassword_EndsExistingSessions()
    {
        var login = await CreateUserAsync();
        using var identity = app.CreateIdentityClient();
        var token = await LoginAsync(identity, login);

        await using (var connection = app.CreateConnection())
        {
            var newHash = new Argon2PasswordHasher().Hash("Changed@1234").Hash;
            await connection.ExecuteAsync("""UPDATE "Users" SET "Password" = @newHash WHERE "Login" = @login""", new { newHash, login });
        }

        identity.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        (await identity.PostAsync("/api/refresh", null, Cancellation)).StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
        using var api = app.CreateClient();
        api.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        (await api.GetAsync("/api/organizations", Cancellation)).StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
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

        using var identity = app.CreateIdentityClient();
        var response = await identity.PostAsJsonAsync("/api/login", new { login, password = Password }, Cancellation);
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task UnknownAndWrongCredentials_LookTheSame()
    {
        var login = await CreateUserAsync();
        using var identity = app.CreateIdentityClient();

        var wrong = await identity.PostAsJsonAsync("/api/login", new { login, password = "Wrong@12345" }, Cancellation);
        var unknown = await identity.PostAsJsonAsync("/api/login", new { login = $"ghost-{Guid.NewGuid():N}", password = "Wrong@12345" }, Cancellation);

        wrong.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        unknown.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        (await wrong.Content.ReadAsStringAsync(Cancellation)).ShouldBe(await unknown.Content.ReadAsStringAsync(Cancellation));
    }

    [Fact]
    public async Task PerAddressRateLimit_RejectsWithRetryAfterAndTheErrorEnvelope()
    {
        await using var limited = app.CreateIdentityFactory(disableRateLimiting: false);
        using var client = limited.CreateClient();
        using var timeout = CancellationTokenSource.CreateLinkedTokenSource(Cancellation);
        timeout.CancelAfter(TimeSpan.FromSeconds(20));

        // 10 permits per window plus a queue of 5: the 16th concurrent request is rejected immediately.
        // Every attempt uses a different login so the per-login lockout can't be what answers 429.
        var requests = Enumerable.Range(0, 16)
            .Select(_ => client.PostAsJsonAsync("/api/login", new { login = $"nobody-{Guid.NewGuid():N}", password = "x" }, timeout.Token))
            .ToArray();
        var pending = requests.ToList();
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

    private static async Task<string> LoginAsync(HttpClient identity, string login)
    {
        var response = await identity.PostAsJsonAsync("/api/login", new { login, password = Password }, Cancellation);
        response.StatusCode.ShouldBe(HttpStatusCode.OK, await response.Content.ReadAsStringAsync(Cancellation));
        return (await response.Content.ReadFromJsonAsync<JsonElement>(Cancellation)).GetProperty("token").GetString()!;
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
            using var identityClient = identity.CreateClient();
            var token = await LoginAsync(identityClient, Infrastructure.Security.AdminLogins.DefaultLogin);
            new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler().ReadJwtToken(token).Claims
                .ShouldContain(claim => claim.Type == "cpnucleo:admin" && claim.Value == "true");

            using var apiClient = api.CreateClient();
            apiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            (await apiClient.GetAsync("/api/users?pageSize=1", Cancellation)).StatusCode.ShouldBe(HttpStatusCode.OK, "user administration is admin-only");
            (await apiClient.PostAsJsonAsync("/api/workflow", new { id = Guid.NewGuid(), name = "Demo admin workflow", order = 1 }, Cancellation))
                .StatusCode.ShouldBe(HttpStatusCode.OK, "catalog writes are admin-only");
        }
    }
}
