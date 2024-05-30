namespace Domain;

public sealed class User : BaseEntity
{
    public string? Name { get; private set; }
    public string? Login { get; private set; }
    public string? Password { get; private set; }
    public string? Salt { get; private set; }

    public static User Create (string name, string login, string password, Ulid id = default)
    {        
        CryptographyManager.CryptPbkdf2(password, out var encryptedPassword, out var salt);

        return new User
        {   
            Id = id == Ulid.Empty ? Ulid.NewUlid () : id,
            Name = name,
            Login = login,
            Password = encryptedPassword,
            Salt = salt,
            CreatedAt = DateTime.UtcNow,
            Active = true
        };
    }

    public static User Update (User obj, string name, string password)
    {
        CryptographyManager.CryptPbkdf2(password, out var encryptedPassword, out var salt);

        obj.Name = name;
        obj.Password = encryptedPassword;
        obj.Salt = salt;
        obj.UpdatedAt = DateTime.UtcNow;

        return obj;
    }
}