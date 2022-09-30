namespace Cpnucleo.Domain.Entities;

public sealed record Projeto : BaseEntity
{
    public string Nome { get; set; }

    public Guid IdSistema { get; set; }

    public Sistema? Sistema { get; set; }
}
