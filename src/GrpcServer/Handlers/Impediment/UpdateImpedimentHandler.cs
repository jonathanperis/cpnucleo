namespace GrpcServer.Handlers.Impediment;

// Dapper Repository Advanced
public sealed class UpdateImpedimentHandler(IUnitOfWork unitOfWork, ILogger<UpdateImpedimentHandler> logger) : ICommandHandler<UpdateImpedimentCommand, UpdateImpedimentResult>
{
    public async Task<UpdateImpedimentResult> ExecuteAsync(UpdateImpedimentCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Service started processing request.");

        try
        {
            logger.LogInformation("Checking if an impediment entity exists with Id: {ImpedimentId}", command.Id);
            var repository = unitOfWork.GetRepository<Domain.Entities.Impediment>();
            var item = await repository.GetByIdAsync(command.Id);

            if (item is null)
            {
                logger.LogWarning("Impediment not found with Id: {ImpedimentId}", command.Id);
                return new UpdateImpedimentResult 
                { 
                    Success = false,
                    Message = "Impediment not found."
                };
            }

            logger.LogInformation("Updating impediment entity with Id: {ImpedimentId}", command.Id);
            Domain.Entities.Impediment.Update(item, command.Name);

            logger.LogInformation("Beginning transaction.");
            await unitOfWork.BeginTransactionAsync();
            
            logger.LogInformation("Updating entity in repository.");
            var success = await repository.UpdateAsync(item);

            logger.LogInformation("Update result: {Success}", success);
            logger.LogInformation("Committing transaction.");
            await unitOfWork.CommitAsync(cancellationToken);

            logger.LogInformation("Service completed successfully.");

            return new UpdateImpedimentResult 
            { 
                Success = success,
                Message = success ? "Impediment updated successfully." : "Failed to update Impediment."
            };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while processing the command. Rolling back transaction.");
            await unitOfWork.RollbackAsync(cancellationToken);
            throw;
        }
    }
}