using Cpnucleo.Infra.Data.Helpers;
using Cpnucleo.Infra.Data.Mappings;
using Microsoft.Extensions.Configuration;

namespace Cpnucleo.Infra.Data.Context;

public class CpnucleoContext : DbContext
{
    public CpnucleoContext()
    { 
    
    }

    //@JONATHAN - Utilizado apenas pelo projeto de teste.
    public CpnucleoContext(DbContextOptions<CpnucleoContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        Guid sistemaId = Guid.NewGuid();
        SistemaMap.Sistemas = new()
        {
            MockEntityHelper.GetNewSistema(sistemaId)
        };

        Guid projetoId = Guid.NewGuid();
        ProjetoMap.Projetos = new()
        {
            MockEntityHelper.GetNewProjeto(sistemaId, projetoId)
        };

        Guid workflowId = Guid.NewGuid();
        WorkflowMap.Workflows = new()
        {
            MockEntityHelper.GetNewWorkflow("Análise", 1, workflowId),
            MockEntityHelper.GetNewWorkflow("Desenvolvimento", 2),
            MockEntityHelper.GetNewWorkflow("Teste", 3),
            MockEntityHelper.GetNewWorkflow("Finalizado", 4)
        };

        Guid recursoId = Guid.NewGuid();
        RecursoMap.Recursos = new()
        {
            MockEntityHelper.GetNewRecurso(recursoId)
        };

        Guid tipoTarefaId = Guid.NewGuid();
        TipoTarefaMap.TipoTarefas = new()
        {
            MockEntityHelper.GetNewTipoTarefa(tipoTarefaId)
        };

        Guid tarefaId = Guid.NewGuid();
        TarefaMap.Tarefas = new()
        {
            MockEntityHelper.GetNewTarefa(projetoId, workflowId, recursoId, tipoTarefaId, tarefaId)
        };

        RecursoTarefaMap.RecursoTarefas = new()
        {
            MockEntityHelper.GetNewRecursoTarefa(tarefaId, recursoId)
        };

        modelBuilder.ApplyConfiguration(new SistemaMap());
        modelBuilder.ApplyConfiguration(new ApontamentoMap());
        modelBuilder.ApplyConfiguration(new ImpedimentoMap());
        modelBuilder.ApplyConfiguration(new ImpedimentoTarefaMap());
        modelBuilder.ApplyConfiguration(new ProjetoMap());
        modelBuilder.ApplyConfiguration(new RecursoMap());
        modelBuilder.ApplyConfiguration(new RecursoProjetoMap());
        modelBuilder.ApplyConfiguration(new RecursoTarefaMap());
        modelBuilder.ApplyConfiguration(new TarefaMap());
        modelBuilder.ApplyConfiguration(new TipoTarefaMap());
        modelBuilder.ApplyConfiguration(new WorkflowMap());

        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder
                .UseSqlite(configuration.GetConnectionString("DefaultConnection"))
                .EnableSensitiveDataLogging();
        }
    }
}