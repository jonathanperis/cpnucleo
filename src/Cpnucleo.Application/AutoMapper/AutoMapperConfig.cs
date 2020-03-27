using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace Cpnucleo.Application.AutoMapper
{
    public static class AutoMapperConfig
    {
        public static void AddAutoMapperSetup(this IServiceCollection services)
        {
            services.AddAutoMapper();
        }
    }
}
