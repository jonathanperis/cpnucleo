namespace Domain.Entities;

[Table("Users")] // Used for Dapper Repository Advanced
public sealed class User : BaseEntity
{
    public string? Name { get; set; }
    public string? Login { get; set; }
    public string? Password { get; set; }
    public string? Salt { get; set; }

    public static User Create(string? name, string? login, PasswordHash passwordHash, Guid id = default)
    {
        var user = new User
        {
            Id = GetNewId(id),
            CreatedAt = DateTime.UtcNow,
            Name = name,
            Login = login,
            Password = passwordHash.Hash,
            Salt = passwordHash.Salt,
            Active = true
        };
        
        return user;
    }

    public static void Update(User obj, string? name, string? login, PasswordHash? passwordHash = null)
    {
        obj.Name = name;
        obj.Login = login;
        if (passwordHash is not null)
        {
            obj.Password = passwordHash.Hash;
            obj.Salt = passwordHash.Salt;
        }
        obj.UpdatedAt = DateTime.UtcNow;
    }

    public static void Remove(User obj)
    {
        obj.Active = false;
        obj.DeletedAt = DateTime.UtcNow;
    }
}
