using Cpnucleo.Application;
using Cpnucleo.Infra.CrossCutting.Bus;
using Cpnucleo.Infra.CrossCutting.Security;
using Cpnucleo.Infra.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Cpnucleo.Infra.CrossCutting.IoC;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddCpnucleoSetup(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddInfraCrossCuttingSecutiry();
        services.AddInfraCrossCuttingBus(configuration);
        services.AddApplication(configuration);
        services.AddInfraData();

        return services;
    }
}