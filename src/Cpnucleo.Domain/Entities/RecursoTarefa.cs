namespace Cpnucleo.Domain.Entities;

public sealed class RecursoTarefa : BaseEntity
{
    public int PercentualTarefa { get; private set; }

    public Guid IdRecurso { get; private set; }

    public Guid IdTarefa { get; private set; }

    public Recurso? Recurso { get; private set; }

    public Tarefa? Tarefa { get; private set; }

    public static RecursoTarefa Create(Guid idRecurso, Guid idTarefa, Guid id = default)
    {
        return new RecursoTarefa
        {
            Id = id == Guid.Empty ? Guid.NewGuid() : id,
            PercentualTarefa = 0,
            IdRecurso = idRecurso,
            IdTarefa = idTarefa,
            DataInclusao = DateTime.UtcNow,
            Ativo = true
        };
    }

    public static RecursoTarefa Update(RecursoTarefa item, Guid idRecurso, Guid idTarefa)
    {
        item.PercentualTarefa = 0;
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
