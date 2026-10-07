using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHealthChecks().AddCheck<DatabaseReadinessCheck>("database", tags: ["ready"]);
        // WebApi traffic crosses Traefik and then the internal NGINX proxy; hosts routed by Traefik
        // alone override this with one hop.
        services.Configure<ForwardedHeadersOptions>(options => options.ForwardLimit = 2);

        // Caller identity and access rules shared by both transports. Hosts that act on behalf of
        // the system (IdentityApi) register their own ICurrentUser before calling this method.
        services.AddHttpContextAccessor();
        services.TryAddScoped<ICurrentUser, HttpContextCurrentUser>();
        services.AddScoped<IAccessGuard, AccessGuard>();
        services.AddScoped<AccessGuardInterceptor>();
        services.AddMemoryCache();
        services.AddSingleton<TokenSessionValidator>();

        services.AddSingleton(_ => NpgsqlDataSource.Create(
            configuration.GetValue<string>("DB_CONNECTION_STRING")
            ?? throw new InvalidOperationException("DB_CONNECTION_STRING configuration is missing.")));

        // EF Core
        services.AddScoped<IApplicationDbContext, ApplicationDbContext>();
        services.AddScoped<ITenantContextAccessor, TenantContextAccessor>();
        services.AddSingleton<IPasswordHasher, Argon2PasswordHasher>();

        // Dapper Repository Basic
        services.AddScoped(provider => provider.GetRequiredService<NpgsqlDataSource>().CreateConnection());
        services.AddScoped<IProjectRepository, ProjectRepository>();

        services.AddScoped<IProjectCreateStore, ProjectCreateStore>();
        services.AddScoped<AccountStore>();

        // Dapper Repository Advanced
        services.AddScoped(provider => new UnitOfWork(
            provider.GetRequiredService<NpgsqlDataSource>().CreateConnection(),
            provider.GetRequiredService<ICurrentUser>(),
            provider.GetRequiredService<IAccessGuard>()));
        services.AddScoped<IUnitOfWork>(provider => provider.GetRequiredService<UnitOfWork>());
    }

    public static void UseInfrastructure(this IApplicationBuilder app)
    {
        app.UseDelta(
            getConnection: httpContext => httpContext.RequestServices.GetRequiredService<NpgsqlConnection>(),
            // Responses are filtered per caller, so validators must be too: otherwise a browser shared
            // by two accounts could revalidate one account's cached list for the other.
            suffix: httpContext => httpContext.User.FindFirst(CpnucleoClaimTypes.Subject)?.Value
                + "|" + httpContext.User.FindFirst(CpnucleoClaimTypes.Admin)?.Value
                + "|" + httpContext.User.FindFirst(CpnucleoClaimTypes.SecurityStamp)?.Value,
            // Live listing streams outlive any single snapshot; ETags only make sense for plain GETs.
            shouldExecute: httpContext => !httpContext.Request.Headers.Accept.Any(value =>
                value?.Contains("text/event-stream", StringComparison.OrdinalIgnoreCase) == true));
    }
}
