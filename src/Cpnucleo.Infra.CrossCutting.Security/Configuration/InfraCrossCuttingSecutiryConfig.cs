namespace Cpnucleo.Infra.CrossCutting.Security.Configuration;

using Cpnucleo.Infra.CrossCutting.Security.Interfaces;
using Microsoft.Extensions.DependencyInjection;

public static class InfraCrossCuttingSecutiryConfig
{
    public static void AddInfraCrossCuttingSecutirySetup(this IServiceCollection services)
    {
        services.AddScoped<ICryptographyManager, CryptographyManager>();
    }
}