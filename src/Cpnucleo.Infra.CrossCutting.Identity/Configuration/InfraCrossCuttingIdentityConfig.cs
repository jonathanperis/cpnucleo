using Cpnucleo.Infra.CrossCutting.Identity.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Cpnucleo.Infra.CrossCutting.Identity.Configuration
{
    public static class InfraCrossCuttingIdentityConfig
    {
        public static void AddInfraCrossCuttingIdentitySetup(this IServiceCollection services)
        {
            services.AddScoped<IClaimsManager, ClaimsManager>();
        }
    }
}
