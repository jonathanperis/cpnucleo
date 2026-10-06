namespace IdentityApi.Security;

/// <summary>
/// Verifies a password with the same Argon2id cost whether or not the login exists, so response
/// timing doesn't reveal which logins are registered.
/// </summary>
public sealed class TimingSafePasswordCheck(IPasswordHasher passwordHasher)
{
    private readonly Lazy<string> unmatchableHash = new(() => passwordHasher.Hash(Guid.NewGuid().ToString("N")).Hash);

    public bool Verify(string password, string? storedHash) =>
        passwordHasher.Verify(password, storedHash ?? unmatchableHash.Value) && storedHash is not null;
}
