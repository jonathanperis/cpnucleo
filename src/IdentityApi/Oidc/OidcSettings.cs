using Open.IdentityServer;
using Open.IdentityServer.Models;

namespace IdentityApi.Oidc;

/// <summary>
/// The OpenID Connect server configuration: lifetimes, API resources and the registered clients.
/// <para>
/// The browser client (<see cref="WebClientId"/>) uses the authorization code flow with PKCE and
/// receives one-time-use refresh tokens bounded by the eight-hour session. Service clients use client
/// credentials and act as a configured service account, so the account's state (active, credentials,
/// admin list) governs them exactly like a person.
/// </para>
/// </summary>
public static class OidcSettings
{
    public static readonly TimeSpan AccessTokenLifetime = TimeSpan.FromMinutes(30);
    public static readonly TimeSpan MaximumSessionLength = TimeSpan.FromHours(8);
    public static readonly TimeSpan IdentityTokenLifetime = TimeSpan.FromMinutes(5);
    public static readonly TimeSpan AuthorizationCodeLifetime = TimeSpan.FromMinutes(1);

    public const string WebClientId = "cpnucleo-webclient";
    public const string WebApiScope = "cpnucleo.api";
    public const string GrpcScope = "cpnucleo.grpc";
    public const string DefaultGrpcAudience = "https://grpc-cpnucleo.jonathanperis.tech";
    public const string DefaultWebClientOrigin = "https://cpnucleo.jonathanperis.tech";

    /// <summary>Client property naming the service account a client-credentials client acts as.</summary>
    public const string ServiceAccountProperty = "cpnucleo:service_login";

    public const int MinimumServiceSecretLength = 32;

    /// <summary>Browser origins allowed to sign in (also the CORS allow-list).</summary>
    public static string[] WebClientOrigins(IConfiguration configuration) =>
        configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() is { Length: > 0 } origins
            ? origins.Select(origin => origin.TrimEnd('/')).ToArray()
            : [DefaultWebClientOrigin];

    /// <summary>The Astro sign-in page the server redirects to (the first WebClient origin by default).</summary>
    public static string LoginPageUrl(IConfiguration configuration) =>
        configuration["Identity:LoginPageUrl"] ?? $"{WebClientOrigins(configuration)[0]}/login/";

    /// <summary>
    /// The sign-in page for a WebClient on <paramref name="redirectUriOrOrigin"/>: <c>{origin}/login/</c>
    /// when that origin is one of <see cref="WebClientOrigins"/>, so each configured WebClient signs in
    /// on its own page. Any other origin (or none) gets <see cref="LoginPageUrl(IConfiguration)"/>, so
    /// the result never points outside the configuration; <c>Identity:LoginPageUrl</c> always wins.
    /// </summary>
    public static string LoginPageUrl(IConfiguration configuration, string? redirectUriOrOrigin)
    {
        if (configuration["Identity:LoginPageUrl"] is not null ||
            !Uri.TryCreate(redirectUriOrOrigin, UriKind.Absolute, out var uri) || uri.Scheme is not ("https" or "http"))
            return LoginPageUrl(configuration);

        var origin = uri.GetLeftPart(UriPartial.Authority);
        var configured = WebClientOrigins(configuration).FirstOrDefault(candidate => string.Equals(candidate, origin, StringComparison.OrdinalIgnoreCase));
        return configured is null ? LoginPageUrl(configuration) : $"{configured}/login/";
    }

    public static string WebApiAudience(IConfiguration configuration) => JwtKeys.Audience(configuration);

    public static string GrpcAudience(IConfiguration configuration) =>
        configuration["Identity:GrpcAudience"] ?? DefaultGrpcAudience;

    public static IEnumerable<IdentityResource> IdentityResources() =>
        [new IdentityResources.OpenId(), new IdentityResources.Profile()];

    public static IEnumerable<ApiScope> ApiScopes() =>
    [
        new(WebApiScope, "Cpnucleo REST API"),
        new(GrpcScope, "Cpnucleo gRPC server")
    ];

    /// <summary>One resource per API host: tokens carry only the audience of the API they were requested for.</summary>
    public static IEnumerable<ApiResource> ApiResources(IConfiguration configuration) =>
    [
        new(WebApiAudience(configuration), "WebApi") { Scopes = { WebApiScope } },
        new(GrpcAudience(configuration), "GrpcServer") { Scopes = { GrpcScope } }
    ];

    public static IEnumerable<Client> Clients(IConfiguration configuration)
    {
        var origins = WebClientOrigins(configuration);
        yield return new Client
        {
            ClientId = WebClientId,
            ClientName = "Cpnucleo WebClient",
            RequireClientSecret = false,
            AllowedGrantTypes = GrantTypes.Code,
            RequirePkce = true,
            AllowPlainTextPkce = false,
            RequireConsent = false,
            AllowAccessTokensViaBrowser = false,
            RedirectUris = origins.Select(origin => $"{origin}/signin-callback/").ToList(),
            PostLogoutRedirectUris = origins.Select(origin => $"{origin}/login/").ToList(),
            AllowedCorsOrigins = origins.ToList(),
            AllowedScopes = { IdentityServerConstants.StandardScopes.OpenId, IdentityServerConstants.StandardScopes.Profile, WebApiScope },
            AllowOfflineAccess = true,
            AccessTokenLifetime = Seconds(AccessTokenLifetime),
            IdentityTokenLifetime = Seconds(IdentityTokenLifetime),
            AuthorizationCodeLifetime = Seconds(AuthorizationCodeLifetime),
            // One refresh token per use, never beyond the session that started at sign-in.
            RefreshTokenUsage = TokenUsage.OneTimeOnly,
            RefreshTokenExpiration = TokenExpiration.Absolute,
            AbsoluteRefreshTokenLifetime = Seconds(MaximumSessionLength),
            UpdateAccessTokenClaimsOnRefresh = true,
            UserSsoLifetime = Seconds(MaximumSessionLength)
        };

        foreach (var section in configuration.GetSection("Identity:ServiceClients").GetChildren())
        {
            var secret = section["Secret"];
            var login = section["Login"];
            if (string.IsNullOrWhiteSpace(secret) || secret.Length < MinimumServiceSecretLength || string.IsNullOrWhiteSpace(login))
                throw new InvalidOperationException(
                    $"Identity:ServiceClients:{section.Key} needs a Secret of at least {MinimumServiceSecretLength} characters and a Login.");

            yield return new Client
            {
                ClientId = section.Key,
                ClientName = section.Key,
                ClientSecrets = { new Secret(secret.Sha256()) },
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                AllowedScopes = (section["Scopes"] ?? $"{WebApiScope} {GrpcScope}")
                    .Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries),
                AccessTokenLifetime = Seconds(AccessTokenLifetime),
                Properties = { [ServiceAccountProperty] = login.Trim() }
            };
        }
    }

    private static int Seconds(TimeSpan value) => (int)value.TotalSeconds;
}
