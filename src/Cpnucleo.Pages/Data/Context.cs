using Cpnucleo.Pages.Models;
using Microsoft.EntityFrameworkCore;

namespace Cpnucleo.Pages.Data
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