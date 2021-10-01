namespace Cpnucleo.Infra.Data.Configuration;

using Cpnucleo.Domain.UoW;
using Cpnucleo.Infra.Data.Context;
using Cpnucleo.Infra.Data.UoW;
using Microsoft.Extensions.DependencyInjection;

public static class InfraDataConfig
{
    public static void AddInfraDataSetup(this IServiceCollection services)
    {
        services.AddScoped<CpnucleoContext>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }
}