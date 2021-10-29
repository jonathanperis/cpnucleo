using Cpnucleo.Application.Configuration;
using Cpnucleo.Domain.Configuration;
using Cpnucleo.Infra.CrossCutting.Bus.Configuration;
using Cpnucleo.Infra.CrossCutting.Security.Configuration;
using Cpnucleo.Infra.Data.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Cpnucleo.Infra.CrossCutting.IoC;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddCpnucleoSetup(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddInfraCrossCuttingSecutirySetup();
        services.AddInfraCrossCuttingBusSetup(configuration);
        services.AddApplicationSetup(configuration);
        services.AddDomainSetup();
        services.AddInfraDataSetup();

        return services;
    }
}