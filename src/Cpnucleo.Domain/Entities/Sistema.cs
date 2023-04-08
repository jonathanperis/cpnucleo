namespace Cpnucleo.Domain.Entities;

public sealed class Sistema : BaseEntity
{
    public string? Nome { get; private set; }

    public string? Descricao { get; private set; }

    public static Sistema Create(string nome, string descricao, Guid id = default)
    {
        return new Sistema
        {
            Id = id == Guid.Empty ? Guid.NewGuid() : id,
            Nome = nome,
            Descricao = descricao,
            DataInclusao = DateTime.UtcNow,
            Ativo = true
        };
    }

    public static Sistema Update(Sistema item, string nome, string descricao)
    {
        item.Nome = nome;
        item.Descricao = descricao;
        item.DataAlteracao = DateTime.UtcNow;

        return item;
    }

    public static Sistema Remove(Sistema item)
    {
        item.Ativo = false;
        item.DataExclusao = DateTime.UtcNow;

        return item;
    }
}
