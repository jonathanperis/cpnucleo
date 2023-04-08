namespace Cpnucleo.Domain.Entities;

public sealed class RecursoTarefa : BaseEntity
{
    public int PercentualTarefa { get; private set; }

    public Guid IdRecurso { get; private set; }

    public Guid IdTarefa { get; private set; }

    public Recurso? Recurso { get; private set; }

    public Tarefa? Tarefa { get; private set; }

    public static RecursoTarefa Create(int percentualTarefa, Guid idRecurso, Guid idTarefa)
    {
        return new RecursoTarefa
        {
            Id = Guid.NewGuid(),
            PercentualTarefa = percentualTarefa,
            IdRecurso = idRecurso,
            IdTarefa = idTarefa,
            DataInclusao = DateTime.UtcNow,
            Ativo = true
        };
    }

    public static RecursoTarefa Update(RecursoTarefa item, int percentualTarefa, Guid idRecurso, Guid idTarefa)
    {
        item.PercentualTarefa = percentualTarefa;
        item.IdRecurso = idRecurso;
        item.IdTarefa = idTarefa;
        item.DataAlteracao = DateTime.UtcNow;

        return item;
    }

    public static RecursoTarefa Remove(RecursoTarefa item)
    {
        item.Ativo = false;
        item.DataExclusao = DateTime.UtcNow;

        return item;
    }
}
