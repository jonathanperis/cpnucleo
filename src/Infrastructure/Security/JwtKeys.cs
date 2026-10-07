using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Security;

/// <summary>
/// Access token validation for the API hosts. IdentityApi is an OpenID Connect provider: it signs
/// RS256 access tokens (<c>typ: at+jwt</c>) with a rotating key ring and publishes the public keys in
/// its discovery document. API hosts download them from <see cref="MetadataAddress"/> (refreshed on
/// an unknown <c>kid</c>), so no host holds key material and rotation needs no redeploy.
/// <para>
/// Issuer, audience, algorithm, token type and lifetime are pinned. Each API host has its own
/// audience (<c>Jwt:Audience</c>): a token requested for WebApi is not accepted by GrpcServer.
/// </para>
/// </summary>
public static class JwtKeys
{
    public const string AccessTokenType = "at+jwt";

    public static string Issuer(IConfiguration configuration) =>
        configuration["Jwt:Issuer"] ?? throw new InvalidOperationException("Jwt:Issuer configuration is missing.");

    public static string Audience(IConfiguration configuration) =>
        configuration["Jwt:Audience"] ?? throw new InvalidOperationException("Jwt:Audience configuration is missing.");

    /// <summary>
    /// The OpenID Connect discovery document. Defaults to the issuer's public address; containers set
    /// <c>Jwt:MetadataAddress</c> to IdentityApi's internal address instead.
    /// </summary>
    public static string MetadataAddress(IConfiguration configuration) =>
        configuration["Jwt:MetadataAddress"] ?? $"{Issuer(configuration).TrimEnd('/')}/.well-known/openid-configuration";

    /// <summary>Plain HTTP is accepted only for an explicitly configured (internal network) metadata address.</summary>
    public static bool RequireHttpsMetadata(IConfiguration configuration) =>
        MetadataAddress(configuration).StartsWith("https://", StringComparison.OrdinalIgnoreCase);

    public static TokenValidationParameters ValidationParameters(IConfiguration configuration) => new()
    {
        ValidAlgorithms = [SecurityAlgorithms.RsaSha256],
        ValidTypes = [AccessTokenType],
        ValidIssuer = Issuer(configuration),
        ValidAudience = Audience(configuration),
        ValidateIssuerSigningKey = true,
        RequireSignedTokens = true,
        ValidateLifetime = true,
        ValidateIssuer = true,
        ValidateAudience = true,
        RequireExpirationTime = true,
        ClockSkew = TimeSpan.FromMinutes(1)
    };
}
