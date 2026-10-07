namespace Infrastructure.Persistence.Projects;

public sealed class ProjectCreateStore(UnitOfWork unitOfWork) : IProjectCreateStore
{
    public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var repository = unitOfWork.GetRepository<Project>();
        return await repository.ExistsAsync(id, cancellationToken).ConfigureAwait(false);
    }

    public async Task<Project> AddAsync(Project project, Guid? creatorUserId, CancellationToken cancellationToken = default)
    {
        var transactionStarted = false;

        try
        {
            await unitOfWork.BeginTransactionAsync(cancellationToken).ConfigureAwait(false);
            transactionStarted = true;

            await unitOfWork.GetRepository<Project>().AddAsync(project, cancellationToken).ConfigureAwait(false);

            // The creator becomes a member so the project stays visible and editable to them. This is
            // a system write: the caller can't add themselves to arbitrary projects through the API.
            // Callers without an active user row (e.g. service tokens) are skipped.
            if (creatorUserId is { } userId)
            {
                var membership = UserProject.Create(userId, project.Id);
                await unitOfWork.ExecuteTrustedAsync("""
                    INSERT INTO "UserProjects" ("Id", "UserId", "ProjectId", "CreatedAt", "Active")
                    SELECT @Id, @UserId, @ProjectId, @CreatedAt, true
                    WHERE EXISTS (SELECT 1 FROM "Users" WHERE "Id" = @UserId AND "Active")
                    """, new { membership.Id, membership.UserId, membership.ProjectId, membership.CreatedAt }, cancellationToken).ConfigureAwait(false);
            }

            await unitOfWork.CommitAsync(cancellationToken).ConfigureAwait(false);
            return project;
        }
        catch
        {
            if (transactionStarted)
            {
                await unitOfWork.RollbackAsync(cancellationToken).ConfigureAwait(false);
            }

            throw;
        }
    }
}
