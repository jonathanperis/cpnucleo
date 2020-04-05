using Cpnucleo.Infra.CrossCutting.Util.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Cpnucleo.Infra.CrossCutting.Util.Configuration
{
    public static class InfraCrossCuttingUtilConfig
    {
        public static void AddInfraCrossCuttingUtilSetup(this IServiceCollection services)
        {
            services.AddScoped<ISystemConfiguration, SystemConfiguration>();
        }
    }
}
