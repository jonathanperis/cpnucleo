using Cpnucleo.Domain.Models;
using Cpnucleo.Infra.Data.Mappings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Cpnucleo.Infra.Data.Context
{
    public class CpnucleoContext : DbContext
    {
        private readonly IHostingEnvironment _env;

        public CpnucleoContext(IHostingEnvironment env)
        {
            _env = env;
        }

        public DbSet<Apontamento> Apontamentos { get; set; }
        public DbSet<Impedimento> Impedimentos { get; set; }
        public DbSet<ImpedimentoTarefa> ImpedimentoTarefas { get; set; }
        public DbSet<Projeto> Projetos { get; set; }
        public DbSet<Recurso> Recursos { get; set; }
        public DbSet<RecursoProjeto> RecursoProjetos { get; set; }
        public DbSet<RecursoTarefa> RecursoTarefas { get; set; }
        public DbSet<Sistema> Sistemas { get; set; }
        public DbSet<Tarefa> Tarefas { get; set; }
        public DbSet<TipoTarefa> TipoTarefas { get; set; }
        public DbSet<Workflow> Workflows { get; set; }

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
            // get the configuration from the app settings
            IConfigurationRoot config = new ConfigurationBuilder()
                .SetBasePath(_env.ContentRootPath)
                .AddJsonFile("appsettings.json")
                .Build();

            // define the database to use
            optionsBuilder.UseSqlServer(config.GetConnectionString("DefaultConnection"));
        }
    }
}