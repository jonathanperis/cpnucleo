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
            var item = await repository.GetByIdAsync(command.Id, cancellationToken);

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
            await unitOfWork.BeginTransactionAsync(cancellationToken);
            
            logger.LogInformation("Updating entity in repository.");
            var success = await repository.UpdateAsync(item, cancellationToken);

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
        catch
        {
            // Rejections and failures are translated (and unexpected ones logged) by the gRPC pipeline.
            await unitOfWork.RollbackAsync(cancellationToken);
            throw;
        }
    }
}