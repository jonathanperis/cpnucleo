using Cpnucleo.Application.Common.Repositories.UoW;
using Cpnucleo.Infrastructure.Data.Context;
using Cpnucleo.Infrastructure.Data.UoW;
using Microsoft.Extensions.DependencyInjection;

namespace Cpnucleo.Infrastructure.Data;

public static class DependencyInjection
{
    public static void AddInfraData(this IServiceCollection services)
    {
        services.AddScoped<CpnucleoContext>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }
}