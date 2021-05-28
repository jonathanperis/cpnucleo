using Cpnucleo.RazorPages.Interfaces;
using Cpnucleo.RazorPages.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Cpnucleo.RazorPages.Configuration
{
    public static class RazorPagesConfig
    {
        public static void AddRazorPagesConfigSetup(this IServiceCollection services)
        {
            services
                .AddScoped<ISistemaService, SistemaService>();
        }
    }
}
