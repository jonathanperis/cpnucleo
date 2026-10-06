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

    public static object Parameters(ICurrentUser currentUser) => new
    {
        AccessAll = currentUser.HasFullAccess,
        AccessUserId = currentUser.UserId ?? Guid.Empty
    };
}
