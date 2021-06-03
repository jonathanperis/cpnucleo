using Cpnucleo.MVC.Interfaces;
using Cpnucleo.MVC.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Cpnucleo.MVC.Configuration
{
    public static class MvcConfig
    {
        public static void AddMvcConfigSetup(this IServiceCollection services)
        {
            services
                .AddScoped<IApontamentoService, ApontamentoService>()
                .AddScoped<IImpedimentoService, ImpedimentoService>()
                .AddScoped<IImpedimentoTarefaService, ImpedimentoTarefaService>()
                .AddScoped<IProjetoService, ProjetoService>()
                .AddScoped<IRecursoProjetoService, RecursoProjetoService>()
                .AddScoped<IRecursoService, RecursoService>()
                .AddScoped<IRecursoTarefaService, RecursoTarefaService>()
                .AddScoped<ISistemaService, SistemaService>()
                .AddScoped<ITarefaService, TarefaService>()
                .AddScoped<ITipoTarefaService, TipoTarefaService>()
                .AddScoped<IWorkflowService, WorkflowService>();
        }
    }
}
