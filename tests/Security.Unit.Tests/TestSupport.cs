using System.Security.Cryptography;
using IdentityApi.Security;
using Microsoft.Extensions.Configuration;

namespace Security.Unit.Tests;

internal sealed class MutableTimeProvider(DateTimeOffset now) : TimeProvider
{
    public DateTimeOffset Now { get; set; } = now;

    public override DateTimeOffset GetUtcNow() => Now;

    public void Advance(TimeSpan by) => Now += by;
}

internal static class TestSupport
{
    public const string SigningKey = "unit-test-only-signing-key-at-least-32-characters";

    public static IConfiguration Configuration(string? adminLogins = null, (string PublicPem, string PrivatePem)? rsa = null)
    {
        var values = new Dictionary<string, string?>
        {
            ["Jwt:SigningKey"] = SigningKey,
            ["Jwt:Issuer"] = "unit-tests",
            ["Jwt:Audience"] = "unit-tests",
            ["CPNUCLEO_ADMIN_LOGINS"] = adminLogins
        };
        if (rsa is { } keys)
        {
            values["Jwt:SigningPublicKey"] = keys.PublicPem;
            values["Jwt:SigningPrivateKey"] = keys.PrivatePem.Replace("\n", "\\n", StringComparison.Ordinal);
        }

        return new ConfigurationBuilder().AddInMemoryCollection(values).Build();
    }

    public static (string PublicPem, string PrivatePem) CreateRsaKeys()
    {
        using var rsa = RSA.Create(2048);
        return (rsa.ExportSubjectPublicKeyInfoPem(), rsa.ExportPkcs8PrivateKeyPem());
    }

    public static ApplicationDbContext CreateDbContext() => new(new DbContextOptionsBuilder<ApplicationDbContext>()
        .UseInMemoryDatabase(Guid.NewGuid().ToString())
        .Options);

    public static TokenIssuer Issuer(IConfiguration configuration, TimeProvider? time = null) =>
        new(configuration, time ?? TimeProvider.System);
}
