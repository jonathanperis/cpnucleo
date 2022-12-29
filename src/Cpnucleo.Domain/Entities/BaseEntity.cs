namespace Cpnucleo.Domain.Entities;

public abstract record BaseEntity
{
    public BaseEntity()
    {
        Ativo = true;
    }

    public Guid Id { get; set; }

    public long ClusteredKey { get; set; }

    public DateTime DataInclusao { get; set; }

    public DateTime? DataAlteracao { get; set; }

    public DateTime? DataExclusao { get; set; }

    public bool Ativo { get; set; }
}
