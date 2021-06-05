using AutoMapper;
using Cpnucleo.Application.Handlers;
using Cpnucleo.Application.Interfaces;
using Cpnucleo.Application.Services;
using Cpnucleo.Domain.Entities;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Sistema.CreateSistema;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Sistema.RemoveSistema;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Sistema.UpdateSistema;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Sistema.GetSistema;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Sistema.ListSistema;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using MediatR;
using MessagePipe;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Cpnucleo.Application.Configuration
{
    public static class ApplicationConfig
    {
        public static void AddApplicationSetup(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(ViewModelToEntityProfile), typeof(EntityToViewModelProfile));

            services
                .AddSingleton<IApontamentoAppService, ApontamentoAppService>()
                .AddSingleton<IImpedimentoAppService, ImpedimentoAppService>()
                .AddSingleton<IImpedimentoTarefaAppService, ImpedimentoTarefaAppService>()
                .AddSingleton<IProjetoAppService, ProjetoAppService>()
                .AddSingleton<IRecursoAppService, RecursoAppService>()
                .AddSingleton<IRecursoProjetoAppService, RecursoProjetoAppService>()
                .AddSingleton<IRecursoTarefaAppService, RecursoTarefaAppService>()
                .AddSingleton<ISistemaAppService, SistemaAppService>()
                .AddSingleton<ITarefaAppService, TarefaAppService>()
                .AddSingleton<ITipoTarefaAppService, TipoTarefaAppService>()
                .AddSingleton<IWorkflowAppService, WorkflowAppService>();

            //services
            //    .AddSingleton<IAsyncRequestHandler<CreateSistemaCommand, CreateSistemaResponse>, SistemaHandler>()
            //    .AddSingleton<IAsyncRequestHandler<ListSistemaQuery, ListSistemaResponse>, SistemaHandler>()
            //    .AddSingleton<IAsyncRequestHandler<GetSistemaQuery, GetSistemaResponse>, SistemaHandler>()
            //    .AddSingleton<IAsyncRequestHandler<RemoveSistemaCommand, RemoveSistemaResponse>, SistemaHandler>()
            //    .AddSingleton<IAsyncRequestHandler<UpdateSistemaCommand, UpdateSistemaResponse>, SistemaHandler>();

            services.AddMessagePipe();
            services.AddMediatR(Assembly.GetExecutingAssembly());
        }
    }

    public class ViewModelToEntityProfile : Profile
    {
        public ViewModelToEntityProfile()
        {
            CreateMap<ApontamentoViewModel, Apontamento>();
            CreateMap<ImpedimentoViewModel, Impedimento>();
            CreateMap<ImpedimentoTarefaViewModel, ImpedimentoTarefa>();
            CreateMap<ProjetoViewModel, Projeto>();
            CreateMap<RecursoViewModel, Recurso>();
            CreateMap<RecursoProjetoViewModel, RecursoProjeto>();
            CreateMap<RecursoTarefaViewModel, RecursoTarefa>();
            CreateMap<SistemaViewModel, Sistema>();
            CreateMap<TarefaViewModel, Tarefa>();
            CreateMap<TipoTarefaViewModel, TipoTarefa>();
            CreateMap<WorkflowViewModel, Workflow>();
        }
    }

    public class EntityToViewModelProfile : Profile
    {
        public EntityToViewModelProfile()
        {
            CreateMap<Apontamento, ApontamentoViewModel>();
            CreateMap<Impedimento, ImpedimentoViewModel>();
            CreateMap<ImpedimentoTarefa, ImpedimentoTarefaViewModel>();
            CreateMap<Projeto, ProjetoViewModel>();
            CreateMap<Recurso, RecursoViewModel>();
            CreateMap<RecursoProjeto, RecursoProjetoViewModel>();
            CreateMap<RecursoTarefa, RecursoTarefaViewModel>();
            CreateMap<Sistema, SistemaViewModel>();
            CreateMap<Tarefa, TarefaViewModel>();
            CreateMap<TipoTarefa, TipoTarefaViewModel>();
            CreateMap<Workflow, WorkflowViewModel>();
        }
    }
}
