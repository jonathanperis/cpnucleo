namespace Cpnucleo.Domain.Entities;

public sealed record TipoTarefa : BaseEntity
{
    public string Nome { get; set; }

    public string Image { get; set; }

    public string? Element { get; set; }
}
