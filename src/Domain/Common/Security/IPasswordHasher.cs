namespace Domain.Common.Security;

public interface IPasswordHasher
{
    PasswordHash Hash(string? password);

    bool Verify(string? password, string? hash);
}
