using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace Cpnucleo.Infra.CrossCutting.Communication.GRPC.AutoMapper
{
    public static class AutoMapperConfig
    {
        public static void AddAutoMapperGrpcClientSetup(this IServiceCollection services)
        {
            services.AddAutoMapper();

            RegisterMappings();
        }

        public static MapperConfiguration RegisterMappings()
        {
            return new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new ModelToViewModelMappingProfile());
                cfg.AddProfile(new ViewModelToModelMappingProfile());
            });
        }
    }
}
