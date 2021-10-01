using Refit;

namespace Cpnucleo.RazorPages.Configuration;

public static class RazorPagesConfig
{
    public static void AddRazorPagesConfigSetup(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddRefitClient<ICpnucleoApiService>()
            .ConfigureHttpClient(c => c.BaseAddress = new Uri($"{configuration.GetValue<string>("AppSettings:UrlCpnucleoApi")}/api/v2"));
    }
}
