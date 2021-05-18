using Cpnucleo.Infra.Data.Mappings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Cpnucleo.Infra.Data.Context
{
    internal class CpnucleoContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public CpnucleoContext(IConfiguration configuration)
        {
            _configuration = configuration;
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
            optionsBuilder
                .UseSqlServer(_configuration.GetConnectionString("DefaultConnection"))
                .EnableSensitiveDataLogging();
        }
    }
}