namespace Domain.Entities;

[Table("Users")]
public sealed class User : BaseEntity
{
    public string? Name { get; set; }
    public string? Login { get; set; }
    public string? Password { get; set; }
    public string? Salt { get; set; }

    public static User Create(string? name, string? login, string? password, Guid id = default)
    {
        CryptographyManager.CryptPbkdf2(password, out var encryptedPassword, out var salt);

        var user = new User
        {
            Id = GetNewId(id),
            CreatedAt = DateTime.UtcNow,
            Name = name,
            Login = login,
            Password = encryptedPassword,
            Salt = salt,
            Active = true
        };
        
        return user;
    }

    public static User Update(User obj, string? name, string? password)
    {
        CryptographyManager.CryptPbkdf2(password, out var encryptedPassword, out var salt);

        obj.Name = name;
        obj.Password = encryptedPassword;
        obj.Salt = salt;
        obj.UpdatedAt = DateTime.UtcNow;

        return obj;
    }

    public static User Remove(User obj)
    {
        obj.Active = false;
        obj.DeletedAt = DateTime.UtcNow;

        return obj;
    }
}
