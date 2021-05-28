using Cpnucleo.RazorPages.Services;
using Cpnucleo.RazorPages.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;

namespace Cpnucleo.RazorPages.Configuration
{
    public static class RazorPagesConfig
    {
        public static void AddRazorPagesConfigSetup(this IServiceCollection services)
        {
            services
                .AddScoped<ICrudService<SistemaViewModel>, SistemaService>();
        }
    }
}
