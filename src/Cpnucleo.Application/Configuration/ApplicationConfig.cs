using AutoMapper;
using Cpnucleo.Domain.Entities;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Cpnucleo.Application.Configuration
{
    public static class ApplicationConfig
    {
        public static void AddApplicationSetup(this IServiceCollection services)
        {
            var assembliesForScanning = new Assembly[]
            {
                typeof(BaseViewModel).GetTypeInfo().Assembly,
                typeof(BaseEntity).GetTypeInfo().Assembly,
            };

            var mappingConfig = new MapperConfiguration(mc => mc.AddMaps(assembliesForScanning));
            services.AddSingleton(mappingConfig.CreateMapper());
        }
    }
}
