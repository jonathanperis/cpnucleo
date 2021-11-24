using Cpnucleo.Application.Events.Sistema;
using Cpnucleo.Application.Hubs;
using Cpnucleo.Application.Interfaces;
using Cpnucleo.Application.Services;
using Cpnucleo.Infra.CrossCutting.Util.Events.Sistema;
using Ev.ServiceBus;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Cpnucleo.Application.Configuration;

public static class ApplicationConfig
{
    public static void AddApplicationSetup(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAutoMapper(typeof(ViewModelToEntityProfile), typeof(EntityToViewModelProfile));

        services
            .AddScoped<IApontamentoAppService, ApontamentoAppService>()
            .AddScoped<IImpedimentoAppService, ImpedimentoAppService>()
            .AddScoped<IImpedimentoTarefaAppService, ImpedimentoTarefaAppService>()
            .AddScoped<IProjetoAppService, ProjetoAppService>()
            .AddScoped<IRecursoAppService, RecursoAppService>()
            .AddScoped<IRecursoProjetoAppService, RecursoProjetoAppService>()
            .AddScoped<IRecursoTarefaAppService, RecursoTarefaAppService>()
            .AddScoped<ISistemaAppService, SistemaAppService>()
            .AddScoped<ITarefaAppService, TarefaAppService>()
            .AddScoped<ITipoTarefaAppService, TipoTarefaAppService>()
            .AddScoped<IWorkflowAppService, WorkflowAppService>();

        services.AddMediatR(Assembly.GetExecutingAssembly());

        services.RegisterServiceBusReception().FromQueue("CpnucleoDefaultQueue", builder =>
        {
            builder.RegisterReception<RemoveSistemaEvent, RemoveSistemaEventHandler>();
        });

        services.AddSignalR()
                .AddAzureSignalR(configuration["AzureSignalR:DefaultConnection"]);
    }

    public static void UseApplicationSetup(this IApplicationBuilder app)
    {
        app.UseFileServer();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapHub<CpnucleoHub>("/hub");
        });
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
