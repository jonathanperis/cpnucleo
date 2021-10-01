using Cpnucleo.Infra.CrossCutting.Security.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Cpnucleo.Infra.CrossCutting.Security.Configuration;

public static class InfraCrossCuttingSecutiryConfig
{
    public static void AddInfraCrossCuttingSecutirySetup(this IServiceCollection services)
    {
        services.AddScoped<ICryptographyManager, CryptographyManager>();
    }
}