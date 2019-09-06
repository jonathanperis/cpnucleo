using Cpnucleo.Pages.Models;
using Microsoft.EntityFrameworkCore;

namespace Cpnucleo.Pages.Data
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options)
            : base(options)
        { }

        public DbSet<ApontamentoModel> Apontamentos { get; set; }
        public DbSet<ImpedimentoModel> Impedimentos { get; set; }
        public DbSet<ImpedimentoTarefaModel> ImpedimentoTarefas { get; set; }
        public DbSet<ProjetoModel> Projetos { get; set; }
        public DbSet<RecursoModel> Recursos { get; set; }
        public DbSet<RecursoProjetoModel> RecursoProjetos { get; set; }
        public DbSet<RecursoTarefaModel> RecursoTarefas { get; set; }
        public DbSet<SistemaModel> Sistemas { get; set; }
        public DbSet<TarefaModel> Tarefas { get; set; }
        public DbSet<TipoTarefaModel> TipoTarefas { get; set; }
        public DbSet<WorkflowModel> Workflows { get; set; }
    }
}