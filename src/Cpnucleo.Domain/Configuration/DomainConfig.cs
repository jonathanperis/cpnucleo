using Cpnucleo.Domain.Entities;
using Cpnucleo.Domain.Interfaces.Services;
using Cpnucleo.Domain.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Cpnucleo.Domain.Configuration
{
    public static class DomainConfig
    {
        public static void AddDomainSetup(this IServiceCollection services)
        {
            services
                .AddScoped<ICrudService<Sistema>, CrudService<Sistema>>()
                .AddScoped<ICrudService<Projeto>, CrudService<Projeto>>()
                .AddScoped<ICrudService<Impedimento>, CrudService<Impedimento>>()
                .AddScoped<ICrudService<TipoTarefa>, CrudService<TipoTarefa>>()
                .AddScoped<ICrudService<Workflow>, CrudService<Workflow>>();

            services
                .AddScoped<ITarefaService, TarefaService>()
                .AddScoped<IApontamentoService, ApontamentoService>()
                .AddScoped<IWorkflowService, WorkflowService>()
                .AddScoped<IRecursoService, RecursoService>()
                .AddScoped<IImpedimentoTarefaService, ImpedimentoTarefaService>()
                .AddScoped<IRecursoProjetoService, RecursoProjetoService>()
                .AddScoped<IRecursoTarefaService, RecursoTarefaService>();
        }
    }
}
