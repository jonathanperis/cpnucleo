using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Security;

/// <summary>
/// Token signing configuration. When <c>Jwt:SigningPublicKey</c> (and, on IdentityApi only,
/// <c>Jwt:SigningPrivateKey</c>) are configured as PEM, tokens use RS256 so API hosts can validate
/// without being able to mint tokens. Otherwise the shared HS256 <c>Jwt:SigningKey</c> is used.
/// PEM values may encode line breaks as <c>\n</c> so they fit in a single environment variable.
/// </summary>
public static class JwtKeys
{
    public const int MinimumSymmetricKeyBytes = 32;

    public static bool UsesAsymmetricSigning(IConfiguration configuration) =>
        !string.IsNullOrWhiteSpace(configuration["Jwt:SigningPublicKey"]);

    public static string Algorithm(IConfiguration configuration) =>
        UsesAsymmetricSigning(configuration) ? SecurityAlgorithms.RsaSha256 : SecurityAlgorithms.HmacSha256;

    public static SecurityKey ValidationKey(IConfiguration configuration)
    {
        if (UsesAsymmetricSigning(configuration))
        {
            // Keep only the public parameters; the temporary RSA object is disposed right away.
            using var rsa = RSA.Create();
            rsa.ImportFromPem(Pem(configuration["Jwt:SigningPublicKey"]!));
            return new RsaSecurityKey(rsa.ExportParameters(includePrivateParameters: false));
        }

        return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SymmetricKey(configuration)));
    }

    /// <summary>The PEM private key (RS256) or shared secret (HS256) used to sign tokens.</summary>
    public static string SigningKey(IConfiguration configuration)
    {
        if (!UsesAsymmetricSigning(configuration)) return SymmetricKey(configuration);
        var privateKey = configuration["Jwt:SigningPrivateKey"];
        if (string.IsNullOrWhiteSpace(privateKey))
            throw new InvalidOperationException("Jwt:SigningPrivateKey is required when Jwt:SigningPublicKey is configured.");
        return Pem(privateKey);
    }

    public static string Issuer(IConfiguration configuration) =>
        configuration["Jwt:Issuer"] ?? throw new InvalidOperationException("Jwt:Issuer configuration is missing.");

    public static string Audience(IConfiguration configuration) =>
        configuration["Jwt:Audience"] ?? throw new InvalidOperationException("Jwt:Audience configuration is missing.");

    public static TokenValidationParameters ValidationParameters(IConfiguration configuration) => new()
    {
        IssuerSigningKey = ValidationKey(configuration),
        ValidAlgorithms = [Algorithm(configuration)],
        ValidIssuer = Issuer(configuration),
        ValidAudience = Audience(configuration),
        ValidateIssuerSigningKey = true,
        ValidateLifetime = true,
        ValidateIssuer = true,
        ValidateAudience = true,
        RequireExpirationTime = true,
        ClockSkew = TimeSpan.FromMinutes(1)
    };

    private static string SymmetricKey(IConfiguration configuration)
    {
        var key = configuration["Jwt:SigningKey"] ?? throw new InvalidOperationException("Jwt:SigningKey configuration is missing.");
        if (Encoding.UTF8.GetByteCount(key) < MinimumSymmetricKeyBytes)
            throw new InvalidOperationException($"Jwt:SigningKey must be at least {MinimumSymmetricKeyBytes} bytes.");
        return key;
    }

    private static string Pem(string value) => value.Replace("\\n", "\n", StringComparison.Ordinal).Trim();
}
