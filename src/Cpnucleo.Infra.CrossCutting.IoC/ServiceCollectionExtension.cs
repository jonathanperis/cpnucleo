using Cpnucleo.Domain.Configuration;
using Cpnucleo.Infra.CrossCutting.Security.Configuration;
using Cpnucleo.Infra.CrossCutting.Util.Configuration;
using Cpnucleo.Infra.Data.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Cpnucleo.Infra.CrossCutting.IoC
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddCpnucleoApiSetup(this IServiceCollection services)
        {
            services.AddDomainSetup();
            services.AddInfraDataSetup();
            services.AddInfraCrossCuttingSecutirySetup();
            services.AddInfraCrossCuttingUtilSetup();

            return services;
        }

        public static IServiceCollection AddCpnucleoSetup(this IServiceCollection services)
        {
            services.AddInfraCrossCuttingUtilSetup();

            return services;
        }
    }
}
