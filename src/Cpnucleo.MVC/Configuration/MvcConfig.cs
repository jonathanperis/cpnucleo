using Cpnucleo.MVC.Services.Interfaces;
using Refit;

namespace Cpnucleo.MVC.Configuration;

public static class MvcConfig
{
    public static void AddMvcConfigSetup(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddRefitClient<ICpnucleoAuthService>()
            .ConfigureHttpClient(c => c.BaseAddress = new Uri($"{configuration.GetValue<string>("AppSettings:UrlCpnucleoApiAuth")}/api/v1"));
    }
}
