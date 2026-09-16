using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Domain.Common.Security;

namespace Security.Unit.Tests;

public class RefreshSessionTests
{
    [TestCase(true, 1, 200)]
    [TestCase(false, 1, 401)]
    [TestCase(true, 9, 401)]
    public async Task Refresh_RechecksTheAccountLifetimeAndPrivileges(bool active, int ageHours, int expectedStatus)
    {
        await using var db = new ApplicationDbContext(new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options);
        var user = User.Create("Learner", "learner", new PasswordHash("unused", ""));
        if (!active) User.Remove(user);
        db.Users!.Add(user);
        await db.SaveChangesAsync(default);
        var endpoint = Factory.Create<IdentityApi.Endpoints.Refresh.Endpoint>(db, LoginEndpointPasswordVerificationTests.TestConfiguration());
        endpoint.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
            new Claim(CpnucleoClaimTypes.Subject, user.Id.ToString()),
            new Claim(CpnucleoClaimTypes.SessionStartedAt, DateTimeOffset.UtcNow.AddHours(-ageHours).ToUnixTimeSeconds().ToString()),
            new Claim(CpnucleoClaimTypes.Admin, "true")
        }, "test"));
        await endpoint.HandleAsync(default);
        endpoint.HttpContext.Response.StatusCode.ShouldBe(expectedStatus);
        if (expectedStatus == 200)
        {
            var token = new JwtSecurityTokenHandler().ReadJwtToken(endpoint.Response.Token);
            token.Subject.ShouldBe(user.Id.ToString());
            token.Claims.ShouldNotContain(c => c.Type == CpnucleoClaimTypes.Admin);
        }
    }
}
