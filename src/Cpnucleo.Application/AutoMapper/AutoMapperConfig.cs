using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace Cpnucleo.Application.AutoMapper
{
    public static class AutoMapperConfig
    {
        public static void AddAutoMapperSetup(this IServiceCollection services)
        {
            RegisterMappings();

            services.AddAutoMapper();
        }

        public static MapperConfiguration RegisterMappings()
        {
            return new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new DomainToViewModelMappingProfile());
                cfg.AddProfile(new ViewModelToDomainMappingProfile());
            });
        }
    }
}
