using Cpnucleo.Domain.Common.Repositories.UoW;
using Cpnucleo.Infrastructure.Context;
using Cpnucleo.Infrastructure.UoW;
using Microsoft.Extensions.DependencyInjection;

namespace Cpnucleo.Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructureData(this IServiceCollection services)
    {
        services.AddScoped<CpnucleoDbContext>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }
}