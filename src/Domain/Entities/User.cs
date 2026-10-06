namespace Domain.Entities;

[Table("Users")] // Used for Dapper Repository Advanced
public sealed class User : BaseEntity
{
    public string? Name { get; private set; }
    public string? Login { get; private set; }
    public string? Password { get; private set; }
    public string? Salt { get; private set; }

    public static User Create(string? name, string? login, PasswordHash passwordHash, Guid id = default)
    {
        ArgumentNullException.ThrowIfNull(passwordHash);
        return new User
        {
            Id = GetNewId(id),
            CreatedAt = DateTime.UtcNow,
            Name = Guard.Required(name, nameof(Name)),
            Login = Guard.Required(login, nameof(Login), Guard.LoginMaxLength),
            Password = passwordHash.Hash,
            Salt = passwordHash.Salt,
            Active = true
        };
    }

    /// <summary>
    /// Updates the profile. A null <paramref name="login"/> keeps the current login and a null
    /// <paramref name="passwordHash"/> keeps the current password.
    /// </summary>
    public static void Update(User obj, string? name, string? login = null, PasswordHash? passwordHash = null)
    {
        obj.Name = Guard.Required(name, nameof(Name));
        if (login is not null) obj.Login = Guard.Required(login, nameof(Login), Guard.LoginMaxLength);
        if (passwordHash is not null)
        {
            obj.Password = passwordHash.Hash;
            obj.Salt = passwordHash.Salt;
        }
        obj.Touch();
    }

    public static void Remove(User obj) => obj.MarkRemoved();
}
