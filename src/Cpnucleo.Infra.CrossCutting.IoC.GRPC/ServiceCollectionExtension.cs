using Cpnucleo.Infra.CrossCutting.Communication.GRPC.Interfaces;
using Cpnucleo.Infra.CrossCutting.Communication.GRPC.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Cpnucleo.Infra.CrossCutting.IoC.GRPC
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddCpnucleoGrpcSetup(this IServiceCollection services)
        {
            // Infra - CrossCutting - Communication - GRPC
            services
                .AddScoped<ISistemaGrpcService, SistemaGrpcService>();

            return services;
        }
    }
}
