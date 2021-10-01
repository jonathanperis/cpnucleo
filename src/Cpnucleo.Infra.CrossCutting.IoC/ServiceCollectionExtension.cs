namespace Cpnucleo.Infra.CrossCutting.IoC;

using Cpnucleo.Application.Configuration;
using Cpnucleo.Domain.Configuration;
using Cpnucleo.Infra.CrossCutting.Security.Configuration;
using Cpnucleo.Infra.Data.Configuration;
using Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddCpnucleoSetup(this IServiceCollection services)
    {
        services.AddApplicationSetup();
        services.AddDomainSetup();
        services.AddInfraDataSetup();
        services.AddInfraCrossCuttingSecutirySetup();

        return services;
    }
}