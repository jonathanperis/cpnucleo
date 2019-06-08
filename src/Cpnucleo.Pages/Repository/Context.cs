using Cpnucleo.Pages.Repository.Apontamento;
using Cpnucleo.Pages.Repository.Impedimento;
using Cpnucleo.Pages.Repository.ImpedimentoTarefa;
using Cpnucleo.Pages.Repository.Projeto;
using Cpnucleo.Pages.Repository.Recurso;
using Cpnucleo.Pages.Repository.RecursoProjeto;
using Cpnucleo.Pages.Repository.RecursoTarefa;
using Cpnucleo.Pages.Repository.Sistema;
using Cpnucleo.Pages.Repository.Tarefa;
using Cpnucleo.Pages.Repository.TipoTarefa;
using Cpnucleo.Pages.Repository.Workflow;
using Microsoft.EntityFrameworkCore;

namespace Cpnucleo.Pages.Repository
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