using System.IdentityModel.Tokens.Jwt;
using Infrastructure.Security;
using Microsoft.IdentityModel.Tokens;

namespace Security.Unit.Tests;

public class JwtSigningTests
{
    // FastEndpoints' token helper resolves its defaults from the FastEndpoints service resolver.
    [OneTimeSetUp]
    public void RegisterFastEndpointsServices() => Factory.RegisterTestServices(_ => { });

    [Test]
    public void Hs256Tokens_ValidateWithTheSharedSecretOnly()
    {
        var configuration = TestSupport.Configuration();
        var token = IssueFor(configuration);

        Validate(token, JwtKeys.ValidationParameters(configuration)).ShouldBeTrue();
        var otherSecret = TestSupport.Configuration();
        otherSecret["Jwt:SigningKey"] = "a-different-secret-that-is-also-32-characters-long";
        Validate(token, JwtKeys.ValidationParameters(otherSecret)).ShouldBeFalse();
    }

    [Test]
    public void Rs256Tokens_ValidateWithThePublicKeyAndRejectSymmetricForgeries()
    {
        var keys = TestSupport.CreateRsaKeys();
        var identity = TestSupport.Configuration(rsa: keys);
        var token = IssueFor(identity);
        new JwtSecurityTokenHandler().ReadJwtToken(token).Header.Alg.ShouldBe(SecurityAlgorithms.RsaSha256);

        // API hosts get only the public key.
        var api = TestSupport.Configuration();
        api["Jwt:SigningPublicKey"] = keys.PublicPem;
        Validate(token, JwtKeys.ValidationParameters(api)).ShouldBeTrue();

        // An HS256 token signed with the (still configured) shared secret must not be accepted.
        Validate(IssueFor(TestSupport.Configuration()), JwtKeys.ValidationParameters(api)).ShouldBeFalse();
    }

    [Test]
    public void ValidationParameters_PinTheAlgorithmIssuerAudienceAndLifetime()
    {
        var parameters = JwtKeys.ValidationParameters(TestSupport.Configuration());

        parameters.ValidAlgorithms.ShouldBe([SecurityAlgorithms.HmacSha256]);
        parameters.ValidateIssuer.ShouldBeTrue();
        parameters.ValidateAudience.ShouldBeTrue();
        parameters.ValidateLifetime.ShouldBeTrue();
        parameters.RequireExpirationTime.ShouldBeTrue();
        parameters.ClockSkew.ShouldBe(TimeSpan.FromMinutes(1));
    }

    [Test]
    public void Configuration_RejectsWeakOrIncompleteKeys()
    {
        var weak = TestSupport.Configuration();
        weak["Jwt:SigningKey"] = "too-short";
        Should.Throw<InvalidOperationException>(() => JwtKeys.ValidationParameters(weak));

        var publicOnly = TestSupport.Configuration();
        publicOnly["Jwt:SigningPublicKey"] = TestSupport.CreateRsaKeys().PublicPem;
        Should.Throw<InvalidOperationException>(() => JwtKeys.SigningKey(publicOnly));
    }

    private static string IssueFor(Microsoft.Extensions.Configuration.IConfiguration configuration) =>
        TestSupport.Issuer(configuration).Issue(
            User.Create("Jane", "jane", new PasswordHash("$argon2id$hash", "")),
            [(CpnucleoClaimTypes.Subject, Guid.NewGuid().ToString())],
            DateTimeOffset.UtcNow.AddMinutes(5));

    private static bool Validate(string token, TokenValidationParameters parameters)
    {
        try
        {
            new JwtSecurityTokenHandler { MapInboundClaims = false }.ValidateToken(token, parameters, out _);
            return true;
        }
        catch (SecurityTokenException)
        {
            return false;
        }
    }
}
