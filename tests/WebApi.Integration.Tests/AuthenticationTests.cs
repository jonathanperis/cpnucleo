using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Dapper;
using Grpc.Core;
using GrpcServer.Contracts.Commands.Organization;
using Microsoft.IdentityModel.Tokens;

namespace WebApi.Integration.Tests;

/// <summary>
/// Token validation is behavioral, not configuration text: forged, expired, revoked and anonymous
/// calls are rejected on both transports.
/// </summary>
[Collection("Database")]
public class AuthenticationTests(WebAppFixture app)
{
    private static CancellationToken Cancellation => TestContext.Current.CancellationToken;

    private static readonly System.Security.Cryptography.RSA Attacker = System.Security.Cryptography.RSA.Create(2048);

    public static TheoryData<string> InvalidTokenReasons =>
    [
        "wrong issuer", "wrong audience", "wrong signature", "HS256 with a shared secret", "not an access token", "expired",
        "stale security stamp", "admin claim without configuration", "no security stamp", "unknown or ended session"
    ];

    /// <summary>A token that fails for <paramref name="reason"/>, minted for the API it is sent to.</summary>
    private static string InvalidToken(string reason, string audience) => reason switch
    {
        "wrong issuer" => WebAppFixture.CreateToken(WebAppFixture.Admin, issuer: "https://someone-else.test", audience: audience),
        "wrong audience" => WebAppFixture.CreateToken(WebAppFixture.Admin, audience: "https://someone-else.test"),
        // Same key id as the real key, so validation can't succeed by picking another key.
        "wrong signature" => WebAppFixture.CreateToken(WebAppFixture.Admin, audience: audience,
            signingKey: new RsaSecurityKey(Attacker) { KeyId = WebAppFixture.SigningKeyId }),
        "HS256 with a shared secret" => WebAppFixture.CreateToken(WebAppFixture.Admin, audience: audience,
            signingKey: new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes("an-attacker-secret-that-is-at-least-32-bytes")),
            algorithm: SecurityAlgorithms.HmacSha256),
        "not an access token" => WebAppFixture.CreateToken(WebAppFixture.Admin, audience: audience, tokenType: "JWT"),
        "expired" => WebAppFixture.CreateToken(WebAppFixture.Admin, audience: audience, expiresAt: DateTime.UtcNow.AddMinutes(-5)),
        "stale security stamp" => WebAppFixture.CreateToken(WebAppFixture.Admin, audience: audience, stamp: "issued-before-a-password-change"),
        "admin claim without configuration" => WebAppFixture.CreateToken(WebAppFixture.Member, audience: audience, adminClaim: true),
        "no security stamp" => WebAppFixture.CreateToken(WebAppFixture.Admin, audience: audience, stamp: ""),
        "unknown or ended session" => WebAppFixture.CreateToken(WebAppFixture.Admin, audience: audience, sessionId: "no-such-session"),
        _ => throw new ArgumentOutOfRangeException(nameof(reason))
    };

    [Theory]
    [MemberData(nameof(InvalidTokenReasons))]
    public async Task InvalidTokens_AreRejectedOnBothTransports(string reason)
    {
        using var client = app.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", InvalidToken(reason, WebAppFixture.Audience));

        var response = await client.GetAsync("/api/organizations?pageSize=1", Cancellation);
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized, reason);
        (await response.Content.ReadFromJsonAsync<JsonElement>(Cancellation)).GetProperty("statusCode").GetInt32().ShouldBe(401);

        var command = new ListOrganizationsCommand { Pagination = new PaginationParams() };
        (await Should.ThrowAsync<RpcException>(() => command.RemoteExecuteAsync(WebAppFixture.GrpcOptions(InvalidToken(reason, WebAppFixture.GrpcAudience)))))
            .StatusCode.ShouldBe(StatusCode.Unauthenticated, reason);
    }

    [Fact]
    public async Task ValidTokens_AreAcceptedOnBothTransports()
    {
        using var client = app.CreateClient(WebAppFixture.Admin);
        (await client.GetAsync("/api/organizations?pageSize=1", Cancellation)).StatusCode.ShouldBe(HttpStatusCode.OK);

        var command = new ListOrganizationsCommand { Pagination = new PaginationParams() };
        (await command.RemoteExecuteAsync(WebAppFixture.GrpcOptions(WebAppFixture.Admin))).ShouldNotBeNull();
    }

    [Fact]
    public async Task AnonymousCalls_AreRejectedOnBothTransports()
    {
        using var client = app.CreateClient();
        (await client.GetAsync("/api/projects", Cancellation)).StatusCode.ShouldBe(HttpStatusCode.Unauthorized);

        var command = new ListOrganizationsCommand { Pagination = new PaginationParams() };
        (await Should.ThrowAsync<RpcException>(() => command.RemoteExecuteAsync(WebAppFixture.GrpcOptions((string?)null))))
            .StatusCode.ShouldBe(StatusCode.Unauthenticated);
    }

    [Fact]
    public async Task DeactivatingAnAccount_RevokesItsTokens()
    {
        var account = TestAccount.Create("leaver");
        await using var connection = app.CreateConnection();
        await connection.ExecuteAsync("""
            INSERT INTO "Users" ("Id", "Name", "Login", "Password", "Salt", "CreatedAt", "Active")
            VALUES (@Id, @Login, @Login, @PasswordHash, '', now(), false)
            """, account);

        using var client = app.CreateClient(account);
        (await client.GetAsync("/api/organizations", Cancellation)).StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task ChangingCredentials_RevokesCachedSessionsWithinTheCacheWindow()
    {
        var account = await CreateActiveAccountAsync("mover");
        using var client = app.CreateClient(account);
        (await client.GetAsync("/api/organizations?pageSize=1", Cancellation)).StatusCode.ShouldBe(HttpStatusCode.OK, "the session is now cached");

        await using (var connection = app.CreateConnection())
            await connection.ExecuteAsync("""UPDATE "Users" SET "Password" = 'changed-' || "Password" WHERE "Id" = @Id""", account);

        // The fixture's cache window is one second (production: 30).
        await Task.Delay(TimeSpan.FromSeconds(1.5), Cancellation);
        (await client.GetAsync("/api/organizations?pageSize=1", Cancellation)).StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task LiveListings_CloseWhenTheSessionIsRevoked()
    {
        var account = await CreateActiveAccountAsync("streamer");
        using var timeout = CancellationTokenSource.CreateLinkedTokenSource(Cancellation);
        timeout.CancelAfter(TimeSpan.FromSeconds(30));
        using var client = app.CreateClient(account);
        using var request = new HttpRequestMessage(HttpMethod.Get, "/api/organizations?pageSize=1");
        request.Headers.Accept.ParseAdd("text/event-stream");
        using var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, timeout.Token);
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        using var reader = new StreamReader(await response.Content.ReadAsStreamAsync(timeout.Token));
        while (await reader.ReadLineAsync(timeout.Token) is { } line && !line.StartsWith("data:", StringComparison.Ordinal)) { }

        await using (var connection = app.CreateConnection())
            await connection.ExecuteAsync("""UPDATE "Users" SET "Active" = false, "DeletedAt" = now() WHERE "Id" = @Id""", account);
        await Task.Delay(TimeSpan.FromSeconds(1.5), Cancellation);
        // Any write wakes the organization stream, which revalidates before sending the next snapshot.
        (await app.Client.PostAsJsonAsync("/api/organization", new { id = Guid.NewGuid(), name = "Wake streams", description = "x" }, Cancellation))
            .StatusCode.ShouldBe(HttpStatusCode.OK);

        var remaining = await reader.ReadToEndAsync(timeout.Token);
        remaining.ShouldNotContain("Wake streams", Case.Sensitive, "a revoked session must not receive further snapshots");
    }

    private async Task<TestAccount> CreateActiveAccountAsync(string name)
    {
        var account = TestAccount.Create(name);
        await using var connection = app.CreateConnection();
        await connection.ExecuteAsync("""
            INSERT INTO "Users" ("Id", "Name", "Login", "Password", "Salt", "CreatedAt", "Active")
            VALUES (@Id, @Login, @Login, @PasswordHash, '', now(), true)
            """, account);
        return account;
    }
}
