using Cpnucleo.Infra.CrossCutting.Security.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Cpnucleo.Infra.CrossCutting.Security;

public static class DependencyInjection
{
    public static void AddInfraCrossCuttingSecutiry(this IServiceCollection services)
    {
        services.AddScoped<ICryptographyManager, CryptographyManager>();
    }
}