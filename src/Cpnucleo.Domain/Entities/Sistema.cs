namespace Cpnucleo.Domain.Entities;

public sealed record Sistema : BaseEntity
{
    public string Nome { get; set; }

    public string Descricao { get; set; }
}
