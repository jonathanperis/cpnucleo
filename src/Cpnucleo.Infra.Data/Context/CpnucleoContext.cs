using Cpnucleo.Infra.CrossCutting.Util.Interfaces;
using Cpnucleo.Infra.Data.Mappings;
using Microsoft.EntityFrameworkCore;

namespace Cpnucleo.Infra.Data.Context
{
    public class CpnucleoContext : DbContext
    {
        private readonly ISystemConfiguration _configuration;

        public CpnucleoContext(ISystemConfiguration configuration)
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
            optionsBuilder.UseSqlServer(_configuration.ConnectionString);
        }
    }
}