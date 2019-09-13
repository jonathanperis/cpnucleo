using Cpnucleo.Application.AutoMapper;
using Cpnucleo.Application.Interfaces;
using Cpnucleo.Application.Services;
using Cpnucleo.Application.ViewModels;
using Cpnucleo.Domain.Interfaces;
using Cpnucleo.Domain.Models;
using Cpnucleo.Infra.CrossCutting.Identity;
using Cpnucleo.Infra.CrossCutting.Identity.Interfaces;
using Cpnucleo.Infra.CrossCutting.Security;
using Cpnucleo.Infra.CrossCutting.Security.Interfaces;
using Cpnucleo.Infra.Data.Context;
using Cpnucleo.Infra.Data.Repository;
using Cpnucleo.Infra.Data.UoW;
using Microsoft.Extensions.DependencyInjection;

namespace Cpnucleo.Infra.CrossCutting.IoC
{
    public static class IServiceCollectionExtension
    {
        public static IServiceCollection AddCpnucleoSetup(this IServiceCollection services)
        {
            // Application
            services
                .AddScoped<IAppService<SistemaViewModel>, AppService<Sistema, SistemaViewModel>>()
                .AddScoped<IAppService<ProjetoViewModel>, AppService<Projeto, ProjetoViewModel>>()
                .AddScoped<IAppService<TarefaViewModel>, AppService<Tarefa, TarefaViewModel>>()
                .AddScoped<IAppService<ApontamentoViewModel>, AppService<Apontamento, ApontamentoViewModel>>()
                .AddScoped<IAppService<WorkflowViewModel>, AppService<Workflow, WorkflowViewModel>>()
                .AddScoped<IAppService<RecursoViewModel>, AppService<Recurso, RecursoViewModel>>()
                .AddScoped<IAppService<ImpedimentoViewModel>, AppService<Impedimento, ImpedimentoViewModel>>()
                .AddScoped<IAppService<ImpedimentoTarefaViewModel>, AppService<ImpedimentoTarefa, ImpedimentoTarefaViewModel>>()
                .AddScoped<IAppService<RecursoProjetoViewModel>, AppService<RecursoProjeto, RecursoProjetoViewModel>>()
                .AddScoped<IAppService<RecursoTarefaViewModel>, AppService<RecursoTarefa, RecursoTarefaViewModel>>()
                .AddScoped<IAppService<TipoTarefaViewModel>, AppService<TipoTarefa, TipoTarefaViewModel>>();

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
                .AddScoped<IRepository<Sistema>, Repository<Sistema>>()
                .AddScoped<IRepository<Projeto>, Repository<Projeto>>()
                .AddScoped<IRepository<Tarefa>, Repository<Tarefa>>()
                .AddScoped<IRepository<Apontamento>, Repository<Apontamento>>()
                .AddScoped<IRepository<Workflow>, Repository<Workflow>>()
                .AddScoped<IRepository<Recurso>, Repository<Recurso>>()
                .AddScoped<IRepository<Impedimento>, Repository<Impedimento>>()
                .AddScoped<IRepository<ImpedimentoTarefa>, Repository<ImpedimentoTarefa>>()
                .AddScoped<IRepository<RecursoProjeto>, Repository<RecursoProjeto>>()
                .AddScoped<IRepository<RecursoTarefa>, Repository<RecursoTarefa>>()
                .AddScoped<IRepository<TipoTarefa>, Repository<TipoTarefa>>();

            services
                .AddScoped<ITarefaRepository, TarefaRepository>()
                .AddScoped<IApontamentoRepository, ApontamentoRepository>()
                .AddScoped<IWorkflowRepository, WorkflowRepository>()
                .AddScoped<IRecursoRepository, RecursoRepository>()
                .AddScoped<IImpedimentoTarefaRepository, ImpedimentoTarefaRepository>()
                .AddScoped<IRecursoProjetoRepository, RecursoProjetoRepository>()
                .AddScoped<IRecursoTarefaRepository, RecursoTarefaRepository>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<CpnucleoContext>();

            // Infra - CrossCutting - Security
            services.AddScoped<ICryptographyManager, CryptographyManager>();

            // Infra - CrossCutting - Identity
            services.AddScoped<IClaimsManager, ClaimsManager>();

            return services;
        }
    }
}
