using Cpnucleo.MVC.Services;
using Cpnucleo.RazorPages.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Cpnucleo.RazorPages.Configuration
{
    public static class MvcConfig
    {
        public static void AddMvcConfigSetup(this IServiceCollection services)
        {
            services
                .AddScoped<ISistemaService, SistemaService>();
        }
    }
}
