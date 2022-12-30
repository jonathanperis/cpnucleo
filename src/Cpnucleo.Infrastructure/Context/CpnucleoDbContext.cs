﻿using Cpnucleo.Infrastructure.Helpers;
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
        SeedData();

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

    private static void SeedData()
    {
        Guid sistemaId = Guid.Parse("b865f5ca-d3c2-46ff-96c8-860207b563c9");
        SistemaMap.Sistemas = new()
        {
            MockEntityHelper.GetNewSistema(sistemaId)
        };

        Guid projetoId = Guid.Parse("38e2dce9-9f48-486a-88c1-12985c3b72ef");
        ProjetoMap.Projetos = new()
        {
            MockEntityHelper.GetNewProjeto(sistemaId, projetoId)
        };

        Guid workflowId = Guid.Parse("0c17bf58-c14b-44de-8883-e7616bf29134");
        WorkflowMap.Workflows = new()
        {
            MockEntityHelper.GetNewWorkflow("Análise", 1, workflowId),
            MockEntityHelper.GetNewWorkflow("Desenvolvimento", 2, Guid.Parse("4562c0b8-d476-46eb-a58d-1ff3a86266ac")),
            MockEntityHelper.GetNewWorkflow("Teste", 3, Guid.Parse("bef7c738-0396-4fe0-af66-a4629261fc8e")),
            MockEntityHelper.GetNewWorkflow("Finalizado", 4, Guid.Parse("d3af39ba-e690-47e5-a40d-0d849e07a294"))
        };

        Guid recursoId = Guid.Parse("ae9cab55-01f8-4bd1-8ca0-92f174bb1aa0");
        RecursoMap.Recursos = new()
        {
            MockEntityHelper.GetNewRecurso(recursoId)
        };

        Guid tipoTarefaId = Guid.Parse("9ecea6c5-dced-44a8-b4cf-9dfcf333fb0a");
        TipoTarefaMap.TiposTarefas = new()
        {
            MockEntityHelper.GetNewTipoTarefa("Projeto", "../gif/gif-projeto.gif", tipoTarefaId),
            MockEntityHelper.GetNewTipoTarefa("Melhoria", "../gif/gif-melhoria.gif", Guid.Parse("a149c773-879d-4b93-905c-2a02835775c1")),
            MockEntityHelper.GetNewTipoTarefa("Problema", "../gif/gif-problema.gif", Guid.Parse("6f1d2369-2879-444c-b50f-1c022f7a40b1")),
            MockEntityHelper.GetNewTipoTarefa("Requisição", "../gif/gif-requisicao.gif", Guid.Parse("eb94316e-a987-4809-9b9d-eb88051f054c"))
        };

        Guid tarefaId = Guid.Parse("82fe661a-620b-40b5-980a-4104b03ce873");
        TarefaMap.Tarefas = new()
        {
            MockEntityHelper.GetNewTarefa(projetoId, workflowId, recursoId, tipoTarefaId, tarefaId)
        };

        RecursoTarefaMap.RecursosTarefas = new()
        {
            MockEntityHelper.GetNewRecursoTarefa(tarefaId, recursoId, Guid.Parse("c38956cb-3f2a-4934-a042-2a8bccdcb2ed"))
        };
    }
}