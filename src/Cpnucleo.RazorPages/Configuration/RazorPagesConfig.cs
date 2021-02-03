using Cpnucleo.RazorPages.Services;
using Cpnucleo.RazorPages.Services.Interfaces;
using Cpnucleo.RazorPages.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace Cpnucleo.RazorPages.Configuration
{
    public static class RazorPagesConfig
    {
        public static void AddRazorPagesConfigSetup(this IServiceCollection services)
        {
            services
                .AddScoped<ICrudService<SistemaViewModel>, SistemaService>()
                .AddScoped<ICrudService<ProjetoViewModel>, ProjetoService>()
                .AddScoped<ICrudService<ImpedimentoViewModel>, ImpedimentoService>()
                .AddScoped<ICrudService<TipoTarefaViewModel>, TipoTarefaService>()
                .AddScoped<ICrudService<WorkflowViewModel>, WorkflowService>();

            services
                .AddScoped<ITarefaService, TarefaService>()
                .AddScoped<IApontamentoService, ApontamentoService>()
                .AddScoped<IRecursoService, RecursoService>()
                .AddScoped<IImpedimentoTarefaService, ImpedimentoTarefaService>()
                .AddScoped<IRecursoProjetoService, RecursoProjetoService>()
                .AddScoped<IRecursoTarefaService, RecursoTarefaService>();
        }
    }
}
