namespace Cpnucleo.Application.Common.Context;

public interface IApplicationDbContext
{
    DbSet<Apontamento> Apontamentos { get; set; }
    DbSet<Impedimento> Impedimentos { get; set; }
    DbSet<ImpedimentoTarefa> ImpedimentoTarefas { get; set; }
    DbSet<Projeto> Projetos { get; set; }
    DbSet<Recurso> Recursos { get; set; }
    DbSet<RecursoProjeto> RecursoProjetos { get; set; }
    DbSet<RecursoTarefa> RecursoTarefas { get; set; }
    DbSet<Sistema> Sistemas { get; set; }
    DbSet<Tarefa> Tarefas { get; set; }
    DbSet<TipoTarefa> TipoTarefas { get; set; }
    DbSet<Workflow> Workflows { get; set; }

    Task<bool> SaveChangesAsync(CancellationToken cancellationToken);
}
