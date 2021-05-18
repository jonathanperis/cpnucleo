using Cpnucleo.RazorPages.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Refit;
using System;

namespace Cpnucleo.RazorPages.Configuration
{
    public static class RazorPagesConfig
    {
        public static void AddRazorPagesConfigSetup(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddRefitClient<ICpnucleoApiService>()
                .ConfigureHttpClient(c => c.BaseAddress = new Uri($"{configuration.GetValue<string>("AppSettings:UrlCpnucleoApi")}/api/v2"));
        }
    }
}
