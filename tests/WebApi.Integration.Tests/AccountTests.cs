using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Dapper;
using Infrastructure.Common.Security;

namespace WebApi.Integration.Tests;

/// <summary>
/// The self-service account endpoints: any signed-in user reads and renames their own account and
/// changes their own password, and nothing else.
/// </summary>
[Collection("Database")]
public class AccountTests(WebAppFixture app)
{
    private const string Password = "Integration@123";
    private const string NewPassword = "Changed@12345";
    private static CancellationToken Cancellation => TestContext.Current.CancellationToken;

    [Fact]
    public async Task Me_ReturnsTheCallersOwnProfile_WithoutCredentials()
    {
        var account = await CreateAccountAsync();
        using var client = Client(account);

        var response = await client.GetAsync("/api/me", Cancellation);

        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        var me = await response.Content.ReadFromJsonAsync<JsonElement>(Cancellation);
        me.GetProperty("id").GetGuid().ShouldBe(account.Id);
        me.GetProperty("name").GetString().ShouldBe("Self service");
        me.GetProperty("login").GetString().ShouldBe(account.Login);
        me.GetProperty("createdAt").GetDateTime().ShouldNotBe(default);
        me.GetProperty("updatedAt").ValueKind.ShouldBe(JsonValueKind.Null);
        me.EnumerateObject().Select(property => property.Name).ShouldBe(["id", "name", "login", "createdAt", "updatedAt"], ignoreOrder: true);

        (await app.Client.GetFromJsonAsync<JsonElement>("/api/me", Cancellation)).GetProperty("id").GetGuid()
            .ShouldBe(WebAppFixture.Admin.Id, "administrators get their own account too");
    }

    [Fact]
    public async Task Me_RequiresAToken()
    {
        using var anonymous = app.CreateClient();

        (await anonymous.GetAsync("/api/me", Cancellation)).StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
        (await anonymous.PatchAsJsonAsync("/api/me", new { name = "Nobody" }, Cancellation)).StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
        var password = await anonymous.PostAsJsonAsync("/api/me/password", new { currentPassword = Password, newPassword = NewPassword }, Cancellation);
        password.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
        (await password.Content.ReadFromJsonAsync<JsonElement>(Cancellation)).GetProperty("statusCode").GetInt32().ShouldBe(401);
    }

    [Fact]
    public async Task Rename_ChangesOnlyTheCallersDisplayName()
    {
        var account = await CreateAccountAsync();
        var bystander = await CreateAccountAsync();
        using var client = Client(account);

        // An id in the body is not part of the contract: the account always comes from the token.
        var response = await client.PatchAsJsonAsync("/api/me", new { id = bystander.Id, name = "  Renamed  ", login = "hijacked" }, Cancellation);

        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        (await response.Content.ReadFromJsonAsync<JsonElement>(Cancellation)).GetProperty("success").GetBoolean().ShouldBeTrue();
        (await RowAsync(account.Id)).ShouldBe(("Renamed", account.Login, account.PasswordHash));
        (await RowAsync(bystander.Id)).ShouldBe(("Self service", bystander.Login, bystander.PasswordHash));
        var me = await client.GetFromJsonAsync<JsonElement>("/api/me", Cancellation);
        me.GetProperty("name").GetString().ShouldBe("Renamed");
        me.GetProperty("updatedAt").ValueKind.ShouldBe(JsonValueKind.String);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public async Task Rename_RequiresAName(string? name)
    {
        var account = await CreateAccountAsync();
        using var client = Client(account);

        var response = await client.PatchAsJsonAsync("/api/me", new { name }, Cancellation);

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        (await response.Content.ReadFromJsonAsync<JsonElement>(Cancellation)).GetProperty("errors").TryGetProperty("name", out _).ShouldBeTrue();
    }

    [Fact]
    public async Task Rename_AppliesTheDomainNameRules()
    {
        var account = await CreateAccountAsync();
        using var client = Client(account);

        var response = await client.PatchAsJsonAsync("/api/me", new { name = new string('n', Domain.Common.Guard.NameMaxLength + 1) }, Cancellation);

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        var body = await response.Content.ReadFromJsonAsync<JsonElement>(Cancellation);
        body.GetProperty("errors").GetProperty("name")[0].GetString().ShouldBe($"Name must be at most {Domain.Common.Guard.NameMaxLength} characters.");
        (await RowAsync(account.Id)).Name.ShouldBe("Self service");
    }

    [Fact]
    public async Task PasswordChange_StoresTheNewPassword_AndRevokesEarlierTokens()
    {
        var account = await CreateAccountAsync();
        var bystander = await CreateAccountAsync();
        using var client = Client(account);

        var response = await client.PostAsJsonAsync("/api/me/password", new { currentPassword = Password, newPassword = NewPassword }, Cancellation);

        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        (await response.Content.ReadFromJsonAsync<JsonElement>(Cancellation)).GetProperty("success").GetBoolean().ShouldBeTrue();
        var hasher = new Argon2PasswordHasher();
        var stored = (await RowAsync(account.Id)).Password;
        hasher.Verify(NewPassword, stored).ShouldBeTrue();
        hasher.Verify(Password, stored).ShouldBeFalse();
        (await RowAsync(bystander.Id)).Password.ShouldBe(bystander.PasswordHash, "other accounts are untouched");

        // The security stamp derives from the hash: tokens issued before the change stop working.
        await Task.Delay(TimeSpan.FromSeconds(1.5), Cancellation);
        (await client.GetAsync("/api/me", Cancellation)).StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
        using var fresh = Client(account with { PasswordHash = stored });
        (await fresh.GetAsync("/api/me", Cancellation)).StatusCode.ShouldBe(HttpStatusCode.OK);
    }

    [Fact]
    public async Task PasswordChange_RejectsAWrongCurrentPassword_OnTheCurrentPasswordField()
    {
        var account = await CreateAccountAsync();
        using var client = Client(account);

        var response = await client.PostAsJsonAsync("/api/me/password", new { currentPassword = "Wrong@12345", newPassword = NewPassword }, Cancellation);

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        var body = await response.Content.ReadFromJsonAsync<JsonElement>(Cancellation);
        body.GetProperty("statusCode").GetInt32().ShouldBe(400);
        body.GetProperty("errors").GetProperty("currentPassword")[0].GetString().ShouldBe("The current password is incorrect.");
        (await RowAsync(account.Id)).Password.ShouldBe(account.PasswordHash);
    }

    public static TheoryData<string, string, string> InvalidPasswordChanges => new()
    {
        { "", NewPassword, "currentPassword" },
        { new string('x', 129), NewPassword, "currentPassword" },
        { Password, "weak", "newPassword" },
        { Password, "", "newPassword" },
        { Password, Password, "newPassword" }
    };

    [Theory]
    [MemberData(nameof(InvalidPasswordChanges))]
    public async Task PasswordChange_ValidatesBothPasswords(string currentPassword, string newPassword, string field)
    {
        var account = await CreateAccountAsync();
        using var client = Client(account);

        var response = await client.PostAsJsonAsync("/api/me/password", new { currentPassword, newPassword }, Cancellation);

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        (await response.Content.ReadFromJsonAsync<JsonElement>(Cancellation)).GetProperty("errors").TryGetProperty(field, out _).ShouldBeTrue();
        (await RowAsync(account.Id)).Password.ShouldBe(account.PasswordHash);
    }

    [Fact]
    public async Task PasswordChange_LocksTheAccountAfterRepeatedWrongPasswords()
    {
        var account = await CreateAccountAsync();
        var bystander = await CreateAccountAsync();
        using var client = Client(account);

        for (var attempt = 0; attempt < WebApi.Common.Services.PasswordChangeThrottle.MaximumFailures; attempt++)
            (await client.PostAsJsonAsync("/api/me/password", new { currentPassword = "Wrong@12345", newPassword = NewPassword }, Cancellation))
                .StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        var locked = await client.PostAsJsonAsync("/api/me/password", new { currentPassword = Password, newPassword = NewPassword }, Cancellation);

        locked.StatusCode.ShouldBe(HttpStatusCode.TooManyRequests, "even the right password waits for the lockout");
        locked.Headers.RetryAfter.ShouldNotBeNull();
        (await locked.Content.ReadFromJsonAsync<JsonElement>(Cancellation)).GetProperty("statusCode").GetInt32().ShouldBe(429);
        (await RowAsync(account.Id)).Password.ShouldBe(account.PasswordHash);

        using var other = Client(bystander);
        (await other.PostAsJsonAsync("/api/me/password", new { currentPassword = Password, newPassword = NewPassword }, Cancellation))
            .StatusCode.ShouldBe(HttpStatusCode.OK, "lockouts are per account");
    }

    private HttpClient Client(TestAccount account)
    {
        var client = app.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", WebAppFixture.CreateToken(account));
        return client;
    }

    private async Task<TestAccount> CreateAccountAsync()
    {
        var account = new TestAccount(Guid.CreateVersion7(), $"self-{Guid.NewGuid():N}@integration.test", new Argon2PasswordHasher().Hash(Password).Hash, IsAdmin: false);
        await using var connection = app.CreateConnection();
        await connection.ExecuteAsync("""
            INSERT INTO "Users" ("Id", "Name", "Login", "Password", "Salt", "CreatedAt", "Active")
            VALUES (@Id, 'Self service', @Login, @PasswordHash, '', now(), true)
            """, account);
        return account;
    }

    private async Task<(string Name, string Login, string Password)> RowAsync(Guid id)
    {
        await using var connection = app.CreateConnection();
        return await connection.QuerySingleAsync<(string, string, string)>(
            """SELECT "Name", "Login", "Password" FROM "Users" WHERE "Id" = @id""", new { id });
    }
}
