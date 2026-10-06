using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using IdentityApi.Security;

namespace Security.Unit.Tests;

public class RefreshSessionTests
{
    [TestCase(true, 1, 200)]
    [TestCase(false, 1, 401)]
    [TestCase(true, 9, 401)]
    [TestCase(true, 8, 401)]
    public async Task Refresh_RechecksTheAccountAndTheSessionBoundary(bool active, int ageHours, int expectedStatus)
    {
        await using var db = TestSupport.CreateDbContext();
        var user = await AddUserAsync(db, active);
        var endpoint = CreateEndpoint(db, TestSupport.Configuration());
        endpoint.HttpContext.User = Principal(user, DateTimeOffset.UtcNow.AddHours(-ageHours), admin: true);

        await endpoint.HandleAsync(default);

        endpoint.HttpContext.Response.StatusCode.ShouldBe(expectedStatus);
        if (expectedStatus == 200)
        {
            var token = new JwtSecurityTokenHandler().ReadJwtToken(endpoint.Response.Token);
            token.Subject.ShouldBe(user.Id.ToString());
            token.Claims.ShouldNotContain(c => c.Type == CpnucleoClaimTypes.Admin, "admin rights are recomputed from configuration");
        }
    }

    [Test]
    public async Task Refresh_RejectsTokensIssuedBeforeACredentialChange()
    {
        await using var db = TestSupport.CreateDbContext();
        var user = await AddUserAsync(db);
        var principal = Principal(user, DateTimeOffset.UtcNow.AddMinutes(-10));
        User.Update(user, user.Name, passwordHash: new PasswordHash("$argon2id$new-hash", string.Empty));
        await db.SaveChangesAsync(default);
        var endpoint = CreateEndpoint(db, TestSupport.Configuration());
        endpoint.HttpContext.User = principal;

        await endpoint.HandleAsync(default);

        endpoint.HttpContext.Response.StatusCode.ShouldBe(401);
    }

    [TestCase(null)]
    [TestCase(30)]
    public async Task Refresh_RejectsMissingOrFutureSessionStarts(int? minutesInTheFuture)
    {
        await using var db = TestSupport.CreateDbContext();
        var user = await AddUserAsync(db);
        var claims = new List<Claim>
        {
            new(CpnucleoClaimTypes.Subject, user.Id.ToString()),
            new(CpnucleoClaimTypes.SecurityStamp, SecurityStamp.Compute(user))
        };
        if (minutesInTheFuture is { } minutes)
            claims.Add(new(CpnucleoClaimTypes.SessionStartedAt, DateTimeOffset.UtcNow.AddMinutes(minutes).ToUnixTimeSeconds().ToString()));
        var endpoint = CreateEndpoint(db, TestSupport.Configuration());
        endpoint.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity(claims, "test"));

        await endpoint.HandleAsync(default);

        endpoint.HttpContext.Response.StatusCode.ShouldBe(401);
    }

    [Test]
    public async Task Refresh_NeverExtendsATokenPastTheEightHourSession()
    {
        await using var db = TestSupport.CreateDbContext();
        var user = await AddUserAsync(db);
        var sessionStart = DateTimeOffset.UtcNow.AddHours(-7.75);
        var endpoint = CreateEndpoint(db, TestSupport.Configuration(adminLogins: user.Login));
        endpoint.HttpContext.User = Principal(user, sessionStart);

        await endpoint.HandleAsync(default);

        endpoint.HttpContext.Response.StatusCode.ShouldBe(200);
        var token = new JwtSecurityTokenHandler().ReadJwtToken(endpoint.Response.Token);
        token.ValidTo.ShouldBe(sessionStart.AddHours(8).UtcDateTime, TimeSpan.FromSeconds(2));
        token.Claims.ShouldContain(c => c.Type == CpnucleoClaimTypes.Admin && c.Value == "true");
    }

    private static ClaimsPrincipal Principal(User user, DateTimeOffset sessionStartedAt, bool admin = false)
    {
        var claims = new List<Claim>
        {
            new(CpnucleoClaimTypes.Subject, user.Id.ToString()),
            new(CpnucleoClaimTypes.SessionStartedAt, sessionStartedAt.ToUnixTimeSeconds().ToString()),
            new(CpnucleoClaimTypes.SecurityStamp, SecurityStamp.Compute(user))
        };
        if (admin) claims.Add(new(CpnucleoClaimTypes.Admin, "true"));
        return new ClaimsPrincipal(new ClaimsIdentity(claims, "test"));
    }

    private static async Task<User> AddUserAsync(ApplicationDbContext db, bool active = true)
    {
        var user = User.Create("Learner", $"learner-{Guid.NewGuid():N}", new PasswordHash("$argon2id$hash", ""));
        if (!active) User.Remove(user);
        db.Users!.Add(user);
        await db.SaveChangesAsync(default);
        return user;
    }

    private static IdentityApi.Endpoints.Refresh.Endpoint CreateEndpoint(ApplicationDbContext db, Microsoft.Extensions.Configuration.IConfiguration configuration) =>
        Factory.Create<IdentityApi.Endpoints.Refresh.Endpoint>(db, TestSupport.Issuer(configuration));
}
