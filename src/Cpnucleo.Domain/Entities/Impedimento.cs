namespace Cpnucleo.Domain.Entities;

public sealed class Impedimento : BaseEntity
{
    public string? Nome { get; private set; }

    public static Impedimento Create(string nome, Guid id = default)
    {
        return new Impedimento
        {
            Id = id == Guid.Empty ? Guid.NewGuid() : id,
            Nome = nome,
            DataInclusao = DateTime.UtcNow,
            Ativo = true
        };
    }

    public static Impedimento Update(Impedimento impedimento, string nome)
    {
        impedimento.Nome = nome;
        impedimento.DataAlteracao = DateTime.UtcNow;

        return impedimento;
    }

    public static Impedimento Remove(Impedimento impedimento)
    {
        impedimento.Ativo = false;
        impedimento.DataExclusao = DateTime.UtcNow;

        return impedimento;
    }
}
