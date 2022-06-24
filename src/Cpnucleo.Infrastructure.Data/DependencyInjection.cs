using Cpnucleo.Application.Common.Repositories.UoW;
using Cpnucleo.Infra.Data.UoW;
using Microsoft.Extensions.DependencyInjection;

namespace Cpnucleo.Infra.Data;

public static class DependencyInjection
{
    public static void AddInfraData(this IServiceCollection services)
    {
        services.AddScoped<CpnucleoContext>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }
}