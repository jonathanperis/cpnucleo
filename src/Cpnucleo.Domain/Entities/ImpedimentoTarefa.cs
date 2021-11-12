namespace Cpnucleo.Domain.Entities;

public class ImpedimentoTarefa : BaseEntity
{
    public string Descricao { get; set; }

    public Guid IdTarefa { get; set; }

    public Guid IdImpedimento { get; set; }

    public Tarefa? Tarefa { get; set; }

    public Impedimento? Impedimento { get; set; }
}
