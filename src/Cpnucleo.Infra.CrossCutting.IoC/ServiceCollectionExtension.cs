using Cpnucleo.Application.AutoMapper;
using Cpnucleo.Application.Interfaces;
using Cpnucleo.Application.Services;
using Cpnucleo.Domain.Entities;
using Cpnucleo.Domain.Interfaces.Repositories;
using Cpnucleo.Infra.CrossCutting.Communication.API.Interfaces;
using Cpnucleo.Infra.CrossCutting.Communication.API.Services;
using Cpnucleo.Infra.CrossCutting.Communication.GRPC.Interfaces;
using Cpnucleo.Infra.CrossCutting.Communication.GRPC.Services;
using Cpnucleo.Infra.CrossCutting.Identity;
using Cpnucleo.Infra.CrossCutting.Identity.Interfaces;
using Cpnucleo.Infra.CrossCutting.Security;
using Cpnucleo.Infra.CrossCutting.Security.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util;
using Cpnucleo.Infra.CrossCutting.Util.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Cpnucleo.Infra.Data.Context;
using Cpnucleo.Infra.Data.Repository;
using Cpnucleo.Infra.Data.UoW;
using Microsoft.Extensions.DependencyInjection;

namespace Cpnucleo.Infra.CrossCutting.IoC
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddCpnucleoSetup(this IServiceCollection services)
        {
            // Application
            services
                .AddScoped<ICrudAppService<SistemaViewModel>, CrudAppService<Sistema, SistemaViewModel>>()
                .AddScoped<ICrudAppService<ProjetoViewModel>, CrudAppService<Projeto, ProjetoViewModel>>()
                .AddScoped<ICrudAppService<ImpedimentoViewModel>, CrudAppService<Impedimento, ImpedimentoViewModel>>()
                .AddScoped<ICrudAppService<TipoTarefaViewModel>, CrudAppService<TipoTarefa, TipoTarefaViewModel>>();

            services
                .AddScoped<ITarefaAppService, TarefaAppService>()
                .AddScoped<IApontamentoAppService, ApontamentoAppService>()
                .AddScoped<IWorkflowAppService, WorkflowAppService>()
                .AddScoped<IRecursoAppService, RecursoAppService>()
                .AddScoped<IImpedimentoTarefaAppService, ImpedimentoTarefaAppService>()
                .AddScoped<IRecursoProjetoAppService, RecursoProjetoAppService>()
                .AddScoped<IRecursoTarefaAppService, RecursoTarefaAppService>();

            services.AddAutoMapperSetup();

            // Infra - Data
            services
                .AddScoped<ICrudRepository<Sistema>, CrudRepository<Sistema>>()
                .AddScoped<ICrudRepository<Projeto>, CrudRepository<Projeto>>()
                .AddScoped<ICrudRepository<Impedimento>, CrudRepository<Impedimento>>()
                .AddScoped<ICrudRepository<TipoTarefa>, CrudRepository<TipoTarefa>>()
                .AddScoped<ICrudRepository<Workflow>, CrudRepository<Workflow>>();

            services
                .AddScoped<ITarefaRepository, TarefaRepository>()
                .AddScoped<IApontamentoRepository, ApontamentoRepository>()
                .AddScoped<IRecursoRepository, RecursoRepository>()
                .AddScoped<IImpedimentoTarefaRepository, ImpedimentoTarefaRepository>()
                .AddScoped<IRecursoProjetoRepository, RecursoProjetoRepository>()
                .AddScoped<IRecursoTarefaRepository, RecursoTarefaRepository>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<CpnucleoContext>();

            // Infra - Security
            services.AddScoped<ICryptographyManager, CryptographyManager>();
            services.AddScoped<IJwtManager, JwtManager>();

            // Infra - CrossCutting - Identity
            services.AddScoped<IClaimsManager, ClaimsManager>();

            // Infra - CrossCutting - Util
            services.AddScoped<ISystemConfiguration, SystemConfiguration>();

            // Infra - CrossCutting - Communication
            services
                .AddScoped<ICrudApiService<SistemaViewModel>, SistemaApiService>()
                .AddScoped<ICrudApiService<ProjetoViewModel>, ProjetoApiService>()
                .AddScoped<ICrudApiService<ImpedimentoViewModel>, ImpedimentoApiService>()
                .AddScoped<ICrudApiService<TipoTarefaViewModel>, TipoTarefaApiService>()
                .AddScoped<ICrudApiService<WorkflowViewModel>, WorkflowApiService>();

            services
                .AddScoped<ITarefaApiService, TarefaApiService>()
                .AddScoped<IApontamentoApiService, ApontamentoApiService>()
                .AddScoped<IRecursoApiService, RecursoApiService>()
                .AddScoped<IImpedimentoTarefaApiService, ImpedimentoTarefaApiService>()
                .AddScoped<IRecursoProjetoApiService, RecursoProjetoApiService>()
                .AddScoped<IRecursoTarefaApiService, RecursoTarefaApiService>();

            // Infra - CrossCutting - Communication - GRPC
            services
                .AddScoped<ICrudGrpcService<SistemaViewModel>, SistemaGrpcService>()
                .AddScoped<ICrudGrpcService<ProjetoViewModel>, ProjetoGrpcService>()
                .AddScoped<ICrudGrpcService<ImpedimentoViewModel>, ImpedimentoGrpcService>()
                .AddScoped<ICrudGrpcService<TipoTarefaViewModel>, TipoTarefaGrpcService>()
                .AddScoped<ICrudGrpcService<WorkflowViewModel>, WorkflowGrpcService>();

            services
                .AddScoped<ITarefaGrpcService, TarefaGrpcService>()
                .AddScoped<IApontamentoGrpcService, ApontamentoGrpcService>()
                .AddScoped<IRecursoGrpcService, RecursoGrpcService>()
                .AddScoped<IImpedimentoTarefaGrpcService, ImpedimentoTarefaGrpcService>()
                .AddScoped<IRecursoProjetoGrpcService, RecursoProjetoGrpcService>()
                .AddScoped<IRecursoTarefaGrpcService, RecursoTarefaGrpcService>();

            return services;
        }
    }
}
