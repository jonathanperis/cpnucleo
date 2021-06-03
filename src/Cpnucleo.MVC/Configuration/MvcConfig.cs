using Cpnucleo.MVC.Services;
using Cpnucleo.MVC.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Cpnucleo.MVC.Configuration
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
