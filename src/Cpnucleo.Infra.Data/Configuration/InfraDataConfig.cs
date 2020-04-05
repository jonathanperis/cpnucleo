using Cpnucleo.Domain.Entities;
using Cpnucleo.Domain.Interfaces.Repositories;
using Cpnucleo.Infra.Data.Context;
using Cpnucleo.Infra.Data.Repository;
using Cpnucleo.Infra.Data.UoW;
using Microsoft.Extensions.DependencyInjection;

namespace Cpnucleo.Infra.Data.Configuration
{
    public static class InfraDataConfig
    {
        public static void AddInfraDataSetup(this IServiceCollection services)
        {
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
        }
    }
}
