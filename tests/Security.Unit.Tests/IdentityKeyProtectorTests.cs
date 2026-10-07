using System.Security.Cryptography;
using System.Text;
using IdentityApi.Oidc;

namespace Security.Unit.Tests;

/// <summary>Signing and Data Protection keys are encrypted at rest with an authenticated cipher.</summary>
public class IdentityKeyProtectorTests
{
    [Test]
    public void ProtectedData_RoundTripsOnlyForTheSamePurpose()
    {
        var protector = new IdentityKeyProtector(TestSupport.Configuration());
        var secret = Encoding.UTF8.GetBytes("private key material");

        var ciphertext = protector.Protect(secret, "signing-key");

        ciphertext.ShouldNotBe(secret);
        protector.Unprotect(ciphertext, "signing-key").ShouldBe(secret);
        Should.Throw<CryptographicException>(() => protector.Unprotect(ciphertext, "data-protection-key"));
    }

    [Test]
    public void TamperedCiphertext_IsRejected()
    {
        var protector = new IdentityKeyProtector(TestSupport.Configuration());
        var ciphertext = protector.Protect(Encoding.UTF8.GetBytes("private key material"), "signing-key");
        ciphertext[^1] ^= 0x01;

        Should.Throw<CryptographicException>(() => protector.Unprotect(ciphertext, "signing-key"));
    }

    [Test]
    public void AnotherSecret_CannotDecrypt()
    {
        var ciphertext = new IdentityKeyProtector(TestSupport.Configuration()).Protect([1, 2, 3], "signing-key");
        var other = new IdentityKeyProtector(TestSupport.Configuration(new Dictionary<string, string?>
        {
            ["Identity:KeyEncryptionSecret"] = "a-completely-different-secret-of-32-bytes"
        }));

        Should.Throw<CryptographicException>(() => other.Unprotect(ciphertext, "signing-key"));
    }

    [Test]
    public void TheExistingJwtSecret_IsTheFallback_AndWeakSecretsAreRefused()
    {
        var fallback = TestSupport.Configuration(new Dictionary<string, string?>
        {
            ["Identity:KeyEncryptionSecret"] = null,
            ["Jwt:SigningKey"] = "an-existing-deployment-secret-of-32-bytes"
        });
        var protector = new IdentityKeyProtector(fallback);
        protector.Unprotect(protector.Protect([7], "signing-key"), "signing-key").ShouldBe(new byte[] { 7 });

        var weak = TestSupport.Configuration(new Dictionary<string, string?> { ["Identity:KeyEncryptionSecret"] = "too-short" });
        Should.Throw<InvalidOperationException>(() => new IdentityKeyProtector(weak));
    }
}
