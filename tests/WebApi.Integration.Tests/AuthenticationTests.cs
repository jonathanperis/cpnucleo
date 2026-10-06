using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Dapper;
using Grpc.Core;
using GrpcServer.Contracts.Commands.Organization;

namespace WebApi.Integration.Tests;

/// <summary>
/// Token validation is behavioral, not configuration text: forged, expired, revoked and anonymous
/// calls are rejected on both transports.
/// </summary>
[Collection("Database")]
public class AuthenticationTests(WebAppFixture app)
{
    private static CancellationToken Cancellation => TestContext.Current.CancellationToken;

    public static TheoryData<string, string> InvalidTokens => new()
    {
        { "wrong issuer", WebAppFixture.CreateToken(WebAppFixture.Admin, issuer: "someone-else") },
        { "wrong audience", WebAppFixture.CreateToken(WebAppFixture.Admin, audience: "someone-else") },
        { "wrong signature", WebAppFixture.CreateToken(WebAppFixture.Admin, signingKey: "an-attacker-key-that-is-at-least-32-characters") },
        { "expired", WebAppFixture.CreateToken(WebAppFixture.Admin, expiresAt: DateTime.UtcNow.AddMinutes(-5)) },
        { "stale security stamp", WebAppFixture.CreateToken(WebAppFixture.Admin, stamp: "issued-before-a-password-change") },
        { "admin claim without configuration", WebAppFixture.CreateToken(WebAppFixture.Member, adminClaim: true) },
        { "no security stamp", WebAppFixture.CreateToken(WebAppFixture.Admin, stamp: "") }
    };

    [Theory]
    [MemberData(nameof(InvalidTokens))]
    public async Task InvalidTokens_AreRejectedOnBothTransports(string reason, string token)
    {
        using var client = app.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await client.GetAsync("/api/organizations?pageSize=1", Cancellation);
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized, reason);
        (await response.Content.ReadFromJsonAsync<JsonElement>(Cancellation)).GetProperty("statusCode").GetInt32().ShouldBe(401);

        var command = new ListOrganizationsCommand { Pagination = new PaginationParams() };
        (await Should.ThrowAsync<RpcException>(() => command.RemoteExecuteAsync(WebAppFixture.GrpcOptions(token))))
            .StatusCode.ShouldBe(StatusCode.Unauthenticated, reason);
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
}
