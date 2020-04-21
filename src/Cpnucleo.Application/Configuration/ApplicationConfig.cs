using AutoMapper;
using Cpnucleo.Application.Interfaces;
using Cpnucleo.Application.Services;
using Cpnucleo.Domain.Entities;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace Cpnucleo.Application.Configuration
{
    public static class ApplicationConfig
    {
        public static void AddApplicationSetup(this IServiceCollection services)
        {
            services
                .AddScoped<ICrudAppService<SistemaViewModel>, CrudAppService<Sistema, SistemaViewModel>>()
                .AddScoped<ICrudAppService<ProjetoViewModel>, CrudAppService<Projeto, ProjetoViewModel>>()
                .AddScoped<ICrudAppService<ImpedimentoViewModel>, CrudAppService<Impedimento, ImpedimentoViewModel>>()
                .AddScoped<ICrudAppService<TipoTarefaViewModel>, CrudAppService<TipoTarefa, TipoTarefaViewModel>>()
                .AddScoped<ICrudAppService<WorkflowViewModel>, CrudAppService<Workflow, WorkflowViewModel>>();

            services
                .AddScoped<ITarefaAppService, TarefaAppService>()
                .AddScoped<IApontamentoAppService, ApontamentoAppService>()
                .AddScoped<IRecursoAppService, RecursoAppService>()
                .AddScoped<IImpedimentoTarefaAppService, ImpedimentoTarefaAppService>()
                .AddScoped<IRecursoProjetoAppService, RecursoProjetoAppService>()
                .AddScoped<IRecursoTarefaAppService, RecursoTarefaAppService>()
                .AddScoped<IWorkflowAppService, WorkflowAppService>();

            services.AddAutoMapper();
        }
    }
}
