using System.IdentityModel.Tokens.Jwt;
using Infrastructure.Security;
using Microsoft.Extensions.Configuration;

namespace Security.Unit.Tests;

public class AdminLoginsTests
{
    [OneTimeSetUp]
    public void RegisterFastEndpointsServices() => Factory.RegisterTestServices(_ => { });

    [TestCase(null)]
    [TestCase("")]
    [TestCase(" , ")]
    public void WithoutConfiguration_TheSeededDemoAccountIsTheAdministrator(string? configured)
    {
        var configuration = Configuration(configured);

        AdminLogins.Contains(configuration, "demo@cpnucleo.local").ShouldBeTrue();
        AdminLogins.Contains(configuration, " DEMO@cpnucleo.local ").ShouldBeTrue("logins compare case- and whitespace-insensitively");
        AdminLogins.Contains(configuration, "someone@else.test").ShouldBeFalse();
    }

    [Test]
    public void AnExplicitList_ReplacesTheDefault()
    {
        var configuration = Configuration("owner@cpnucleo.test, ops@cpnucleo.test");

        AdminLogins.Contains(configuration, "ops@cpnucleo.test").ShouldBeTrue();
        AdminLogins.Contains(configuration, "demo@cpnucleo.local").ShouldBeFalse("listing administrators removes the default one");
    }

    [Test]
    public void TheDemoAccountReceivesTheAdminClaimByDefault()
    {
        var demo = User.Create("Cpnucleo Demo", AdminLogins.DefaultLogin, new PasswordHash("$argon2id$hash", ""));

        var token = TestSupport.Issuer(TestSupport.Configuration()).Issue(demo,
            [(CpnucleoClaimTypes.Subject, demo.Id.ToString())], DateTimeOffset.UtcNow.AddMinutes(5));

        new JwtSecurityTokenHandler().ReadJwtToken(token).Claims
            .ShouldContain(claim => claim.Type == CpnucleoClaimTypes.Admin && claim.Value == "true");
    }

    private static IConfiguration Configuration(string? adminLogins) =>
        new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string?> { [AdminLogins.ConfigurationKey] = adminLogins }).Build();
}
