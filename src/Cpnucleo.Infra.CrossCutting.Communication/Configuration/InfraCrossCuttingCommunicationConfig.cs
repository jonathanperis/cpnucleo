using AutoMapper;
using Cpnucleo.Infra.CrossCutting.Communication.API.Interfaces;
using Cpnucleo.Infra.CrossCutting.Communication.API.Services;
using Cpnucleo.Infra.CrossCutting.Communication.GRPC.Interfaces;
using Cpnucleo.Infra.CrossCutting.Communication.GRPC.Services;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace Cpnucleo.Infra.CrossCutting.Communication.Configuration
{
    public static class InfraCrossCuttingCommunicationConfig
    {
        public static void AddInfraCrossCuttingCommunicationSetup(this IServiceCollection services)
        {
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

            services.AddAutoMapper();
        }
    }
}
