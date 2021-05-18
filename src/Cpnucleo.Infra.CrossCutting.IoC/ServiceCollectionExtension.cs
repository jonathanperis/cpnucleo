using Cpnucleo.Domain.Configuration;
using Cpnucleo.Infra.CrossCutting.Security.Configuration;
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

            return services;
        }
    }
}
