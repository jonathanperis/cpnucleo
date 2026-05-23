namespace Infrastructure.Persistence.Projects;

public sealed class ProjectCreateStore(IUnitOfWork unitOfWork) : IProjectCreateStore
{
    public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var repository = unitOfWork.GetRepository<Project>();
        return await repository.ExistsAsync(id);
    }

    public async Task<Project> AddAsync(Project project, CancellationToken cancellationToken = default)
    {
        var repository = unitOfWork.GetRepository<Project>();

        try
        {
            await unitOfWork.BeginTransactionAsync();
            var createdId = await repository.AddAsync(project);
            await unitOfWork.CommitAsync(cancellationToken);

            var createdItem = await repository.GetByIdAsync(createdId);
            return createdItem!;
        }
        catch
        {
            await unitOfWork.RollbackAsync(cancellationToken);
            throw;
        }
    }
}
