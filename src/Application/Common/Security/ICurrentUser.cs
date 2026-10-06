namespace Application.Common.Security;

/// <summary>
/// The caller of the current operation.
/// </summary>
public interface ICurrentUser
{
    /// <summary>The authenticated user id (JWT <c>sub</c>), or null when there is no authenticated caller.</summary>
    Guid? UserId { get; }

    /// <summary>True when the token carries <c>cpnucleo:admin=true</c>.</summary>
    bool IsAdmin { get; }

    /// <summary>
    /// True for trusted in-process work with no request at all (migrations, seeding, tests that use
    /// repositories directly). Requests always run as a user, even when unauthenticated.
    /// </summary>
    bool IsSystem { get; }

    /// <summary>Unrestricted access: administrators and trusted system work.</summary>
    bool HasFullAccess => IsSystem || IsAdmin;
}

/// <summary>An explicit caller, used by tools and tests.</summary>
public sealed record StaticCurrentUser(Guid? UserId, bool IsAdmin, bool IsSystem) : ICurrentUser
{
    public static StaticCurrentUser System { get; } = new(null, false, true);

    public static StaticCurrentUser Member(Guid userId) => new(userId, false, false);

    public static StaticCurrentUser Administrator(Guid userId) => new(userId, true, false);
}
