using Cpnucleo.Application.Configuration;
using Cpnucleo.Domain.Configuration;
using Cpnucleo.Infra.CrossCutting.Communication.Configuration;
using Cpnucleo.Infra.CrossCutting.Identity.Configuration;
using Cpnucleo.Infra.CrossCutting.Security.Configuration;
using Cpnucleo.Infra.CrossCutting.Util.Configuration;
using Cpnucleo.Infra.Data.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Cpnucleo.Infra.CrossCutting.IoC
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddCpnucleoApiSetup(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddApplicationSetup();
            services.AddDomainSetup();
            services.AddInfraDataSetup();
            services.AddInfraCrossCuttingSecutirySetup(configuration);
            services.AddInfraCrossCuttingUtilSetup();

            return services;
        }

        public static IServiceCollection AddCpnucleoSetup(this IServiceCollection services)
        {
            services.AddInfraCrossCuttingIdentitySetup();
            services.AddInfraCrossCuttingUtilSetup();
            services.AddInfraCrossCuttingCommunicationSetup();

            return services;
        }
    }
}
