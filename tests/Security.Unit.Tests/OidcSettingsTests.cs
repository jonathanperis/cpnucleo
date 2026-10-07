using IdentityApi.Oidc;
using Open.IdentityServer.Models;

namespace Security.Unit.Tests;

/// <summary>The registered clients and resources encode the security model of the identity server.</summary>
public class OidcSettingsTests
{
    private static readonly Dictionary<string, string?> TwoOrigins = new()
    {
        ["Cors:AllowedOrigins:0"] = "https://web.unit.test",
        ["Cors:AllowedOrigins:1"] = "http://localhost:5400/"
    };

    [Test]
    public void TheWebClient_UsesAuthorizationCodeWithPkce_AndNoSecret()
    {
        var client = WebClient(TestSupport.Configuration(TwoOrigins));

        client.AllowedGrantTypes.ShouldBe([GrantType.AuthorizationCode]);
        client.RequirePkce.ShouldBeTrue();
        client.AllowPlainTextPkce.ShouldBeFalse();
        client.RequireClientSecret.ShouldBeFalse("a browser app cannot keep a secret");
        client.AllowAccessTokensViaBrowser.ShouldBeFalse("no implicit or hybrid tokens in URLs");
        client.RedirectUris.ShouldBe(["https://web.unit.test/signin-callback/", "http://localhost:5400/signin-callback/"], ignoreOrder: true);
        client.PostLogoutRedirectUris.ShouldBe(["https://web.unit.test/login/", "http://localhost:5400/login/"], ignoreOrder: true);
        client.AllowedCorsOrigins.ShouldBe(["https://web.unit.test", "http://localhost:5400"], ignoreOrder: true);
        client.AllowedScopes.ShouldBe(["openid", "profile", OidcSettings.WebApiScope], ignoreOrder: true);
    }

    [Test]
    public void TheWebClient_GetsOneTimeRefreshTokensBoundedByTheEightHourSession()
    {
        var client = WebClient(TestSupport.Configuration());

        client.AllowOfflineAccess.ShouldBeTrue();
        client.RefreshTokenUsage.ShouldBe(TokenUsage.OneTimeOnly);
        client.RefreshTokenExpiration.ShouldBe(TokenExpiration.Absolute);
        client.AbsoluteRefreshTokenLifetime.ShouldBe((int)TimeSpan.FromHours(8).TotalSeconds);
        client.AccessTokenLifetime.ShouldBe((int)TimeSpan.FromMinutes(30).TotalSeconds);
        client.UpdateAccessTokenClaimsOnRefresh.ShouldBeTrue("refreshes recompute the admin claim and stamp");
    }

    [Test]
    public void EachApi_IsItsOwnResource()
    {
        var configuration = TestSupport.Configuration(new Dictionary<string, string?> { ["Identity:GrpcAudience"] = "https://grpc.unit.test" });

        var resources = OidcSettings.ApiResources(configuration).ToDictionary(resource => resource.Name);

        resources[TestSupport.Audience].Scopes.ShouldBe([OidcSettings.WebApiScope]);
        resources["https://grpc.unit.test"].Scopes.ShouldBe([OidcSettings.GrpcScope]);
    }

    [Test]
    public void ServiceClients_UseClientCredentialsAsTheirServiceAccount()
    {
        var configuration = TestSupport.Configuration(new Dictionary<string, string?>
        {
            ["Identity:ServiceClients:lab-tool:Secret"] = new string('s', 40),
            ["Identity:ServiceClients:lab-tool:Login"] = " robot@cpnucleo.test ",
            ["Identity:ServiceClients:lab-tool:Scopes"] = OidcSettings.GrpcScope
        });

        var client = OidcSettings.Clients(configuration).Single(candidate => candidate.ClientId == "lab-tool");

        client.AllowedGrantTypes.ShouldBe([GrantType.ClientCredentials]);
        client.AllowedScopes.ShouldBe([OidcSettings.GrpcScope]);
        client.ClientSecrets.ShouldHaveSingleItem().Value.ShouldNotBe(new string('s', 40), "only the hash is kept");
        client.Properties[OidcSettings.ServiceAccountProperty].ShouldBe("robot@cpnucleo.test");
    }

    [TestCase("short-secret", "robot@cpnucleo.test")]
    [TestCase("a-secret-that-is-long-enough-for-a-client", null)]
    public void ServiceClients_RequireAStrongSecretAndAnAccount(string secret, string? login)
    {
        var configuration = TestSupport.Configuration(new Dictionary<string, string?>
        {
            ["Identity:ServiceClients:lab-tool:Secret"] = secret,
            ["Identity:ServiceClients:lab-tool:Login"] = login
        });

        Should.Throw<InvalidOperationException>(() => OidcSettings.Clients(configuration).ToList());
    }

    [TestCase("http://localhost:5400/signin-callback/", "http://localhost:5400/login/")]
    [TestCase("https://WEB.unit.test/signin-callback/", "https://web.unit.test/login/")]
    [TestCase("https://attacker.test/signin-callback/", "https://web.unit.test/login/")]
    [TestCase("https://web.unit.test.attacker.test/signin-callback/", "https://web.unit.test/login/")]
    [TestCase("http://localhost:5401/signin-callback/", "https://web.unit.test/login/")]
    [TestCase("javascript:alert(1)", "https://web.unit.test/login/")]
    [TestCase("/signin-callback/", "https://web.unit.test/login/")]
    [TestCase(null, "https://web.unit.test/login/")]
    public void TheSignInPage_IsOnTheWebClientOriginThatAsked_AndNeverOutsideTheConfiguredOrigins(string? redirectUri, string expected)
    {
        OidcSettings.LoginPageUrl(TestSupport.Configuration(TwoOrigins), redirectUri).ShouldBe(expected);
    }

    [Test]
    public void AnExplicitLoginPageUrl_AlwaysWins()
    {
        var configuration = TestSupport.Configuration(new Dictionary<string, string?>(TwoOrigins) { ["Identity:LoginPageUrl"] = "https://login.unit.test/sign-in/" });

        OidcSettings.LoginPageUrl(configuration, "http://localhost:5400/signin-callback/").ShouldBe("https://login.unit.test/sign-in/");
        OidcSettings.LoginPageUrl(configuration).ShouldBe("https://login.unit.test/sign-in/");
    }

    private static Client WebClient(Microsoft.Extensions.Configuration.IConfiguration configuration) =>
        OidcSettings.Clients(configuration).Single(client => client.ClientId == OidcSettings.WebClientId);
}
