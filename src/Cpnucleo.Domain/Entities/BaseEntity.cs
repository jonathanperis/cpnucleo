namespace Cpnucleo.Domain.Entities;

public abstract class BaseEntity
{
    public Guid Id { get; protected set; }

    public long ClusteredKey { get; protected set; }

    public DateTime DataInclusao { get; protected set; }

    public DateTime? DataAlteracao { get; protected set; }

    public DateTime? DataExclusao { get; protected set; }

    public bool Ativo { get; protected set; }
}
