using Domain.Common.Security;

namespace WebApi.Integration.Tests.Hosts;

/// <summary>A seeded account whose tokens pass session validation (active user + matching stamp).</summary>
public sealed record TestAccount(Guid Id, string Login, string PasswordHash, bool IsAdmin)
{
    public string Stamp => SecurityStamp.Compute(PasswordHash, Login);

    public static TestAccount Create(string name, bool isAdmin = false) =>
        new(Guid.CreateVersion7(), $"{name}-{Guid.NewGuid():N}@integration.test", $"$argon2id$integration-{Guid.NewGuid():N}", isAdmin);
}
