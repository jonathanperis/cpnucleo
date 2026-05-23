namespace Infrastructure.Persistence.Projects;

public sealed class ProjectCreateStore(IUnitOfWork unitOfWork) : IProjectCreateStore
{
    public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var repository = unitOfWork.GetRepository<Project>();
        return await repository.ExistsAsync(id).ConfigureAwait(false);
    }

    public async Task<Project> AddAsync(Project project, CancellationToken cancellationToken = default)
    {
        var transactionStarted = false;

        try
        {
            await unitOfWork.BeginTransactionAsync().ConfigureAwait(false);
            transactionStarted = true;

            var repository = unitOfWork.GetRepository<Project>();
            var createdId = await repository.AddAsync(project).ConfigureAwait(false);
            await unitOfWork.CommitAsync(cancellationToken).ConfigureAwait(false);
            transactionStarted = false;

            var createdItem = await repository.GetByIdAsync(createdId).ConfigureAwait(false);
            return createdItem ?? throw new InvalidOperationException($"Project {createdId} was not found after creation.");
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
