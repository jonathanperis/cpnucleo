namespace Infrastructure.Security;

/// <summary>
/// Write-side enforcement of <see cref="ResourceAccess"/>, shared by EF Core (through
/// <see cref="AccessGuardInterceptor"/>) and the Dapper repositories, and therefore by REST and gRPC.
/// </summary>
public sealed class AccessGuard(ICurrentUser currentUser, NpgsqlDataSource dataSource) : IAccessGuard
{
    public async Task EnsureCanWriteAsync(AccessTarget target, AccessOperation operation, CancellationToken cancellationToken = default)
    {
        if (currentUser.HasFullAccess) return;
        var userId = currentUser.UserId ?? throw new AccessDeniedException("Authentication is required.");

        switch (ResourceAccess.KindOf(target.EntityType))
        {
            case ResourceAccessKind.Reference:
                throw new AccessDeniedException("Only administrators can change shared catalog data.");
            case ResourceAccessKind.UserAdministration:
                throw new AccessDeniedException("User administration requires an administrator.");
            case ResourceAccessKind.Project when operation == AccessOperation.Create:
                break;
            case ResourceAccessKind.Project:
            case ResourceAccessKind.ProjectScoped:
                await RequireAsync(IsProjectMemberSql, userId, target.ProjectId, cancellationToken).ConfigureAwait(false);
                break;
            case ResourceAccessKind.AssignmentScoped:
                await RequireAsync(IsAssignmentMemberSql, userId, target.AssignmentId, cancellationToken).ConfigureAwait(false);
                break;
        }

        if (ResourceAccess.IsOwnedByUser(target.EntityType) && target.OwnerUserId != userId)
            throw new AccessDeniedException("You can only record and change your own appointments.");
    }

    private const string IsProjectMemberSql = """
        SELECT EXISTS (SELECT 1 FROM "UserProjects"
                       WHERE "UserId" = @UserId AND "ProjectId" = @Id AND "Active")
        """;

    private const string IsAssignmentMemberSql = """
        SELECT EXISTS (SELECT 1 FROM "Assignments" a
                       JOIN "UserProjects" up ON up."ProjectId" = a."ProjectId" AND up."Active"
                       WHERE a."Id" = @Id AND up."UserId" = @UserId)
        """;

    private async Task RequireAsync(string sql, Guid userId, Guid? id, CancellationToken cancellationToken)
    {
        if (id is not { } value || value == Guid.Empty)
            throw new AccessDeniedException("You are not a member of the project.");

        await using var connection = await dataSource.OpenConnectionAsync(cancellationToken).ConfigureAwait(false);
        var isMember = await connection.ExecuteScalarAsync<bool>(new CommandDefinition(
            sql, new { UserId = userId, Id = value }, cancellationToken: cancellationToken)).ConfigureAwait(false);
        if (!isMember) throw new AccessDeniedException("You are not a member of the project.");
    }
}
