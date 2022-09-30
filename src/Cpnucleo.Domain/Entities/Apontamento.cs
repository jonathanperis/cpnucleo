namespace Cpnucleo.Domain.Entities;

public sealed record Apontamento : BaseEntity
{
    public string Descricao { get; set; }

    public DateTime? DataApontamento { get; set; }

    public int QtdHoras { get; set; }

    public Guid IdTarefa { get; set; }

    public Guid IdRecurso { get; set; }

    public Tarefa? Tarefa { get; set; }
}
