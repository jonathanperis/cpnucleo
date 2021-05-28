using AutoMapper;
using Cpnucleo.Application.Interfaces;
using Cpnucleo.Application.Services;
using Cpnucleo.Domain.Entities;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Cpnucleo.Application.Configuration
{
    public static class ApplicationConfig
    {
        public static void AddApplicationSetup(this IServiceCollection services)
        {
            Assembly[] assembliesForScanning = new Assembly[]
            {
                typeof(SistemaViewModel).GetTypeInfo().Assembly,
                typeof(Sistema).GetTypeInfo().Assembly,
            };

            MapperConfiguration mappingConfig = new MapperConfiguration(mc => mc.AddMaps(assembliesForScanning));
            services.AddSingleton(mappingConfig.CreateMapper());

            services.AddScoped<ISistemaAppService, SistemaAppService>();

            services.AddMediatR(Assembly.GetExecutingAssembly());
        }
    }
}
