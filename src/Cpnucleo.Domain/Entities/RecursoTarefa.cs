namespace Cpnucleo.Domain.Entities;

public sealed record RecursoTarefa : BaseEntity
{
    public int PercentualTarefa { get; set; }

    public Guid IdRecurso { get; set; }

    public Guid IdTarefa { get; set; }

    public Recurso? Recurso { get; set; }

    public Tarefa? Tarefa { get; set; }
}
