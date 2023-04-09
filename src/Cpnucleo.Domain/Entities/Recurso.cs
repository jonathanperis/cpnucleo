namespace Cpnucleo.Domain.Entities;

public sealed class Recurso : BaseEntity
{
    public string? Nome { get; private set; }

    public string? Login { get; private set; }

    public string? Senha { get; private set; }

    public string? Salt { get; private set; }

    public static Recurso Create(string nome, string login, string senha, Guid id = default)
    {
        CryptographyManager.CryptPbkdf2(senha, out string senhaCrypt, out string salt);

        return new Recurso
        {
            Id = id == Guid.Empty ? Guid.NewGuid() : id,
            Nome = nome,
            Login = login,
            Senha = senhaCrypt,
            Salt = salt,
            DataInclusao = DateTime.UtcNow,
            Ativo = true
        };
    }

    public static Recurso Update(Recurso item, string nome, string senha)
    {
        CryptographyManager.CryptPbkdf2(senha, out string senhaCrypt, out string salt);

        item.Nome = nome;
        item.Senha = senhaCrypt;
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

    public static bool VerifyPassword(string senha, string senhaCrypt, string salt)
    {
        return CryptographyManager.VerifyPbkdf2(senha, senhaCrypt, salt);
    }
}
