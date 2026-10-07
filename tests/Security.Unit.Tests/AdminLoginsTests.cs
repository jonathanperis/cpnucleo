using Infrastructure.Security;
using Microsoft.Extensions.Configuration;

namespace Security.Unit.Tests;

public class AdminLoginsTests
{
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

    private static IConfiguration Configuration(string? adminLogins) =>
        new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string?> { [AdminLogins.ConfigurationKey] = adminLogins }).Build();
}
