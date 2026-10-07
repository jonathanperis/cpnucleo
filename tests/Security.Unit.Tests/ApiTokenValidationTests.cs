using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Infrastructure.Security;
using Microsoft.IdentityModel.Tokens;

namespace Security.Unit.Tests;

/// <summary>What the API hosts accept: RS256 access tokens from the identity server, nothing else.</summary>
public class ApiTokenValidationTests
{
    private static readonly RSA IdentityKey = RSA.Create(2048);

    [OneTimeTearDown]
    public void DisposeKey() => IdentityKey.Dispose();

    [Test]
    public void ValidationParameters_PinAlgorithmTypeIssuerAudienceAndLifetime()
    {
        var parameters = JwtKeys.ValidationParameters(TestSupport.Configuration());

        parameters.ValidAlgorithms.ShouldBe([SecurityAlgorithms.RsaSha256]);
        parameters.ValidTypes.ShouldBe(["at+jwt"]);
        parameters.ValidIssuer.ShouldBe(TestSupport.Issuer);
        parameters.ValidAudience.ShouldBe(TestSupport.Audience);
        parameters.RequireSignedTokens.ShouldBeTrue();
        parameters.ValidateLifetime.ShouldBeTrue();
        parameters.RequireExpirationTime.ShouldBeTrue();
        parameters.ClockSkew.ShouldBe(TimeSpan.FromMinutes(1));
        parameters.IssuerSigningKey.ShouldBeNull("keys come from the discovery document, not configuration");
    }

    [Test]
    public void Metadata_DefaultsToTheIssuerAndAllowsHttpOnlyForAnExplicitInternalAddress()
    {
        var defaults = TestSupport.Configuration();
        JwtKeys.MetadataAddress(defaults).ShouldBe($"{TestSupport.Issuer}/.well-known/openid-configuration");
        JwtKeys.RequireHttpsMetadata(defaults).ShouldBeTrue();

        var internalAddress = TestSupport.Configuration(new Dictionary<string, string?>
        {
            ["Jwt:MetadataAddress"] = "http://identityapi-cpnucleo:5010/.well-known/openid-configuration"
        });
        JwtKeys.MetadataAddress(internalAddress).ShouldStartWith("http://identityapi-cpnucleo:5010/");
        JwtKeys.RequireHttpsMetadata(internalAddress).ShouldBeFalse();
    }

    [Test]
    public void AnRs256AccessToken_FromTheIdentityKey_IsAccepted()
    {
        Validate(Token(new RsaSecurityKey(IdentityKey), SecurityAlgorithms.RsaSha256, "at+jwt")).ShouldBeTrue();
    }

    [Test]
    public void ForgedOrMistypedTokens_AreRejected()
    {
        using var attacker = RSA.Create(2048);
        Validate(Token(new RsaSecurityKey(attacker), SecurityAlgorithms.RsaSha256, "at+jwt")).ShouldBeFalse("signed by another key");
        Validate(Token(new SymmetricSecurityKey(Encoding.UTF8.GetBytes("a-shared-secret-that-is-at-least-32-bytes")), SecurityAlgorithms.HmacSha256, "at+jwt"))
            .ShouldBeFalse("HS256 is not an accepted algorithm");
        Validate(Token(new RsaSecurityKey(IdentityKey), SecurityAlgorithms.RsaSha256, "JWT"))
            .ShouldBeFalse("an identity token or other JWT is not an access token");
        Validate(Token(new RsaSecurityKey(IdentityKey), SecurityAlgorithms.RsaSha256, "at+jwt", audience: "https://grpc.unit.test"))
            .ShouldBeFalse("tokens for another API are rejected");
    }

    private static string Token(SecurityKey key, string algorithm, string type, string audience = TestSupport.Audience)
    {
        var handler = new JwtSecurityTokenHandler();
        var token = handler.CreateJwtSecurityToken(new SecurityTokenDescriptor
        {
            Issuer = TestSupport.Issuer,
            Audience = audience,
            Subject = new ClaimsIdentity([new Claim("sub", Guid.NewGuid().ToString())]),
            Expires = DateTime.UtcNow.AddMinutes(5),
            SigningCredentials = new SigningCredentials(key, algorithm),
            TokenType = type
        });
        return handler.WriteToken(token);
    }

    private static bool Validate(string token)
    {
        var parameters = JwtKeys.ValidationParameters(TestSupport.Configuration());
        // The JWKS key the discovery document would provide.
        parameters.IssuerSigningKey = new RsaSecurityKey(IdentityKey.ExportParameters(false));
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
