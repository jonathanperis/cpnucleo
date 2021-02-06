using Cpnucleo.RazorPages.Services;
using Cpnucleo.RazorPages.Services.Interfaces;
using Cpnucleo.RazorPages.Models;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using System;

namespace Cpnucleo.RazorPages.Configuration
{
    public static class RazorPagesConfig
    {
        public static void AddRazorPagesConfigSetup(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddScoped<IHttpService, HttpService>()
                .AddScoped(sp => new HttpClient { BaseAddress = new Uri($"{configuration.GetValue<string>("AppSettings:UrlCpnucleoApi")}/api/v2/") });                
        }
    }
}
