namespace Infrastructure.Security;

/// <summary>
/// Write-side enforcement of <see cref="ResourceAccess"/>, shared by EF Core (through
/// <see cref="AccessGuardInterceptor"/>) and the Dapper repositories, and therefore by REST and gRPC.
/// </summary>
public sealed class AccessGuard(ICurrentUser currentUser, NpgsqlDataSource dataSource) : IAccessGuard
{
    public async Task EnsureCanWriteAsync(AccessTarget target, AccessOperation operation, DatabaseSession? session = null, CancellationToken cancellationToken = default)
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
                await RequireAsync(IsProjectMemberSql, userId, target.ProjectId, operation, session, cancellationToken).ConfigureAwait(false);
                break;
            case ResourceAccessKind.AssignmentScoped:
                await RequireAsync(IsAssignmentMemberSql, userId, target.AssignmentId, operation, session, cancellationToken).ConfigureAwait(false);
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

    private async Task RequireAsync(string sql, Guid userId, Guid? id, AccessOperation operation, DatabaseSession? session, CancellationToken cancellationToken)
    {
        if (id is { } value && value != Guid.Empty && await IsMemberAsync(sql, userId, value, session, cancellationToken).ConfigureAwait(false))
            return;

        // An existing row of a project the caller doesn't belong to is invisible to them: report it
        // as missing, exactly like the read paths do. Targeting a foreign project is a denial.
        if (operation == AccessOperation.Modify) throw new RecordNotFoundException();
        throw new AccessDeniedException("You are not a member of the project.");
    }

    private async Task<bool> IsMemberAsync(string sql, Guid userId, Guid id, DatabaseSession? session, CancellationToken cancellationToken)
    {
        var parameters = new { UserId = userId, Id = id };
        if (session is not null)
        {
            // Same connection and transaction as the write: no second pooled connection, and rows the
            // caller's transaction already wrote are visible.
            return await session.Connection.ExecuteScalarAsync<bool>(new CommandDefinition(
                sql, parameters, session.Transaction, cancellationToken: cancellationToken)).ConfigureAwait(false);
        }

        await using var connection = await dataSource.OpenConnectionAsync(cancellationToken).ConfigureAwait(false);
        return await connection.ExecuteScalarAsync<bool>(new CommandDefinition(
            sql, parameters, cancellationToken: cancellationToken)).ConfigureAwait(false);
    }
}
