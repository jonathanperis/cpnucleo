namespace GrpcServer.Handlers.Project;

// Dapper Repository Advanced
public sealed class UpdateProjectHandler(IUnitOfWork unitOfWork, ILogger<UpdateProjectHandler> logger) : ICommandHandler<UpdateProjectCommand, UpdateProjectResult>
{
    public async Task<UpdateProjectResult> ExecuteAsync(UpdateProjectCommand command, CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation("Checking if an project entity exists with Id: {ProjectId}", command.Id);
            var repository = unitOfWork.GetRepository<Domain.Entities.Project>();
            var item = await repository.GetByIdAsync(command.Id, cancellationToken);

            if (item is null)
            {
                logger.LogWarning("Project not found with Id: {ProjectId}", command.Id);
                return new UpdateProjectResult 
                { 
                    Success = false,
                    Message = "Project not found."
                };
            }

            logger.LogInformation("Updating project entity with Id: {ProjectId}", command.Id);
            Domain.Entities.Project.Update(item, command.Name, command.OrganizationId);

            logger.LogInformation("Beginning transaction.");
            await unitOfWork.BeginTransactionAsync(cancellationToken);
            
            logger.LogInformation("Updating entity in repository.");
            var success = command.ExpectedVersion is { } version
                ? await repository.UpdateIfVersionAsync(item, version, cancellationToken)
                : await repository.UpdateAsync(item, cancellationToken);

            logger.LogInformation("Update result: {Success}", success);
            logger.LogInformation("Committing transaction.");
            await unitOfWork.CommitAsync(cancellationToken);

            return new UpdateProjectResult 
            { 
                Success = success,
                Message = success ? "Project updated successfully." : "The project changed. Reload before saving your changes."
            };
        }
        catch
        {
            // Rejections and failures are translated (and unexpected ones logged) by the gRPC pipeline.
            await unitOfWork.RollbackAsync(cancellationToken);
            throw;
        }
    }
}
