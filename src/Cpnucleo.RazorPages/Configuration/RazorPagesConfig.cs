using Cpnucleo.RazorPages.Services;
using Refit;

namespace Cpnucleo.RazorPages.Configuration;

public static class RazorPagesConfig
{
    public static void AddRazorPagesConfigSetup(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddRefitClient<ICpnucleoAuthApiClient>()
            .ConfigureHttpClient(c => c.BaseAddress = new Uri($"{configuration.GetValue<string>("AppSettings:UrlCpnucleoApiAuth")}/api/v1"));

        services
            .AddRefitClient<ICpnucleoApiClient>()
            .ConfigureHttpClient(c => c.BaseAddress = new Uri($"{configuration.GetValue<string>("AppSettings:UrlCpnucleoApi")}/api/v3"))
            .AddHttpMessageHandler<AuthHeaderHandler>();
    }
}
