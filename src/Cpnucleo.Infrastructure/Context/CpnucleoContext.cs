﻿using Cpnucleo.Infrastructure.Mappings;
using Microsoft.Extensions.Configuration;

namespace Cpnucleo.Infrastructure.Context;

public sealed class CpnucleoContext : DbContext
{
    private readonly IConfiguration _configuration;

    public CpnucleoContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public CpnucleoContext(DbContextOptions<CpnucleoContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
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
                .UseSqlServer(_configuration.GetConnectionString("DefaultConnection"))
                .EnableSensitiveDataLogging();
        }
    }
}