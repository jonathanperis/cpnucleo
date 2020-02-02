using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace Cpnucleo.GRPC.AutoMapper
{
    public static class AutoMapperConfig
    {
        public static void AddAutoMapperGrpcServerSetup(this IServiceCollection services)
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
