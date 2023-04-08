namespace Cpnucleo.Domain.Entities;

public sealed class Projeto : BaseEntity
{
    public string? Nome { get; private set; }

    public Guid IdSistema { get; private set; }

    public Sistema? Sistema { get; private set; }

    public static Projeto Create(string nome, Guid idSistema)
    {
        return new Projeto
        {
            Id = Guid.NewGuid(),
            Nome = nome,
            IdSistema = idSistema,
            DataInclusao = DateTime.UtcNow,
            Ativo = true
        };
    }

    public static Projeto Update(Projeto item, string nome, Guid idSistema)
    {
        item.Nome = nome;
        item.IdSistema = idSistema;
        item.DataAlteracao = DateTime.UtcNow;

        return item;
    }

    public static Projeto Remove(Projeto item)
    {
        item.Ativo = false;
        item.DataExclusao = DateTime.UtcNow;

        return item;
    }
}
