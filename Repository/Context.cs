using dotnet_cpnucleo_pages.Repository.Apontamento;
using dotnet_cpnucleo_pages.Repository.Impedimento;
using dotnet_cpnucleo_pages.Repository.ImpedimentoTarefa;
using dotnet_cpnucleo_pages.Repository.Projeto;
using dotnet_cpnucleo_pages.Repository.Recurso;
using dotnet_cpnucleo_pages.Repository.RecursoProjeto;
using dotnet_cpnucleo_pages.Repository.RecursoTarefa;
using dotnet_cpnucleo_pages.Repository.Sistema;
using dotnet_cpnucleo_pages.Repository.Tarefa;
using dotnet_cpnucleo_pages.Repository.TipoTarefa;
using dotnet_cpnucleo_pages.Repository.Workflow;
using Microsoft.EntityFrameworkCore;

namespace dotnet_cpnucleo_pages.Repository
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options)
            : base(options)
        { }

        public DbSet<ApontamentoItem> Apontamentos { get; set; }
        public DbSet<ImpedimentoItem> Impedimentos { get; set; }
        public DbSet<ImpedimentoTarefaItem> ImpedimentoTarefas { get; set; }
        public DbSet<ProjetoItem> Projetos { get; set; }
        public DbSet<RecursoItem> Recursos { get; set; }
        public DbSet<RecursoProjetoItem> RecursoProjetos { get; set; }
        public DbSet<RecursoTarefaItem> RecursoTarefas { get; set; }
        public DbSet<SistemaItem> Sistemas { get; set; }
        public DbSet<TarefaItem> Tarefas { get; set; }
        public DbSet<TipoTarefaItem> TipoTarefas { get; set; }
        public DbSet<WorkflowItem> Workflows { get; set; }
    }
}