namespace Cpnucleo.Domain.Entities;

public class Workflow : BaseEntity
{
    public string Nome { get; set; }

    public int? Ordem { get; set; }

    public string? TamanhoColuna { get; set; }
}
