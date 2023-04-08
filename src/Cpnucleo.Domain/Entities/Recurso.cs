namespace Cpnucleo.Domain.Entities;

public sealed class Recurso : BaseEntity
{
    public string? Nome { get; private set; }

    public string? Login { get; private set; }

    public string? Senha { get; private set; }

    public string? Salt { get; private set; }

    public static Recurso Create(string nome, string login, string senha, string salt)
    {
        return new Recurso
        {
            Id = Guid.NewGuid(),
            Nome = nome,
            Login = login,
            Senha = senha,
            Salt = salt,
            DataInclusao = DateTime.UtcNow,
            Ativo = true
        };
    }

    public static Recurso Update(Recurso item, string nome, string login, string senha, string salt)
    {
        item.Nome = nome;
        item.Login = login;
        item.Senha = senha;
        item.Salt = salt;
        item.DataAlteracao = DateTime.UtcNow;

        return item;
    }

    public static Recurso Remove(Recurso item)
    {
        item.Ativo = false;
        item.DataExclusao = DateTime.UtcNow;

        return item;
    }
}
