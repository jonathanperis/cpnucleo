namespace Infrastructure.Security;

/// <summary>
/// SQL visibility predicates derived from <see cref="ResourceAccess"/>. They expect the
/// <c>@AccessAll</c> and <c>@AccessUserId</c> parameters produced by <see cref="Parameters"/>.
/// </summary>
public static class AccessSql
{
    private const string MemberProjects =
        """SELECT up."ProjectId" FROM "UserProjects" up WHERE up."UserId" = @AccessUserId AND up."Active" """;

    private const string MemberAssignments =
        """SELECT a."Id" FROM "Assignments" a JOIN "UserProjects" up ON up."ProjectId" = a."ProjectId" AND up."Active" WHERE up."UserId" = @AccessUserId""";

    /// <summary>An <c>AND (...)</c> clause restricting rows of <paramref name="entityType"/> to what the caller may read.</summary>
    public static string ReadPredicate(Type entityType) => ResourceAccess.KindOf(entityType) switch
    {
        ResourceAccessKind.Reference => string.Empty,
        ResourceAccessKind.UserAdministration => """ AND (@AccessAll OR "Id" = @AccessUserId)""",
        ResourceAccessKind.Project => $""" AND (@AccessAll OR "Id" IN ({MemberProjects}))""",
        ResourceAccessKind.ProjectScoped => $""" AND (@AccessAll OR "ProjectId" IN ({MemberProjects}))""",
        ResourceAccessKind.AssignmentScoped => $""" AND (@AccessAll OR "AssignmentId" IN ({MemberAssignments}))""",
        var kind => throw new InvalidOperationException($"Unsupported access kind {kind}.")
    };

    /// <summary>
    /// Like <see cref="ReadPredicate"/>, for soft-deleted rows of <paramref name="table"/> about to be
    /// restored. Removing a project also removes its memberships, so a member still sees a project
    /// whose membership was removed together with it (same <c>DeletedAt</c>); restoring the project
    /// brings that membership back.
    /// </summary>
    public static string RestorePredicate(Type entityType, string table) => ResourceAccess.KindOf(entityType) switch
    {
        ResourceAccessKind.Project => $"""
             AND (@AccessAll OR "Id" IN (SELECT up."ProjectId" FROM "UserProjects" up
                                         WHERE up."UserId" = @AccessUserId
                                           AND (up."Active" OR up."DeletedAt" = "{table}"."DeletedAt")))
            """,
        _ => ReadPredicate(entityType)
    };

    public static object Parameters(ICurrentUser currentUser) => new
    {
        AccessAll = currentUser.HasFullAccess,
        AccessUserId = currentUser.UserId ?? Guid.Empty
    };
}
