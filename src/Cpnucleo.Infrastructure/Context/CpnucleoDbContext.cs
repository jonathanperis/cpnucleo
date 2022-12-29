using Cpnucleo.Infrastructure.Helpers;
using Cpnucleo.Infrastructure.Mappings;
using Microsoft.Extensions.Configuration;

namespace Cpnucleo.Infrastructure.Context;

public sealed class CpnucleoDbContext : DbContext
{
    private readonly IConfiguration _configuration;

    public CpnucleoDbContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public CpnucleoDbContext(DbContextOptions<CpnucleoDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //SeedData();

        modelBuilder.ApplyConfiguration(new ApontamentoMap());
        modelBuilder.ApplyConfiguration(new ImpedimentoMap());
        modelBuilder.ApplyConfiguration(new ImpedimentoTarefaMap());
        modelBuilder.ApplyConfiguration(new ProjetoMap());
        modelBuilder.ApplyConfiguration(new RecursoMap());
        modelBuilder.ApplyConfiguration(new RecursoProjetoMap());
        modelBuilder.ApplyConfiguration(new RecursoTarefaMap());
        modelBuilder.ApplyConfiguration(new SistemaMap());
        modelBuilder.ApplyConfiguration(new TarefaMap());
        modelBuilder.ApplyConfiguration(new TipoTarefaMap());
        modelBuilder.ApplyConfiguration(new WorkflowMap());

        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder
                .UseNpgsql(_configuration.GetConnectionString("DefaultConnection"));
        }
    }

    private void SeedData()
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
        TipoTarefaMap.TiposTarefas = new()
        {
            MockEntityHelper.GetNewTipoTarefa(tipoTarefaId)
        };

        Guid tarefaId = Guid.NewGuid();
        TarefaMap.Tarefas = new()
        {
            MockEntityHelper.GetNewTarefa(projetoId, workflowId, recursoId, tipoTarefaId, tarefaId)
        };

        RecursoTarefaMap.RecursosTarefas = new()
        {
            MockEntityHelper.GetNewRecursoTarefa(tarefaId, recursoId)
        };
    }
}