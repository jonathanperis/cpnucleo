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
    public const string Issuer = "https://identity.unit.test";
    public const string Audience = "https://api.unit.test";
    public const string KeyEncryptionSecret = "unit-test-only-key-encryption-secret-32+";

    public static IConfiguration Configuration(IDictionary<string, string?>? overrides = null)
    {
        var values = new Dictionary<string, string?>
        {
            ["Jwt:Issuer"] = Issuer,
            ["Jwt:Audience"] = Audience,
            ["Identity:KeyEncryptionSecret"] = KeyEncryptionSecret
        };
        foreach (var (key, value) in overrides ?? new Dictionary<string, string?>()) values[key] = value;
        return new ConfigurationBuilder().AddInMemoryCollection(values).Build();
    }
}
