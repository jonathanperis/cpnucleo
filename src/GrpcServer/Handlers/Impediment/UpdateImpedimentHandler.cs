namespace GrpcServer.Handlers.Impediment;

// EF Core
public sealed class UpdateImpedimentHandler(IApplicationDbContext dbContext, ILogger<UpdateImpedimentHandler> logger) : ICommandHandler<UpdateImpedimentCommand, UpdateImpedimentResult>
{
    public async Task<UpdateImpedimentResult> ExecuteAsync(UpdateImpedimentCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Service started processing request.");

        try
        {
            logger.LogInformation("Checking if an impediment entity exists with Id: {ImpedimentId}", command.Id);
            var item = await dbContext.Impediments!.FindAsync([command.Id, cancellationToken], cancellationToken: cancellationToken);

            if (item is null)
            {
                logger.LogWarning("Impediment not found with Id: {ImpedimentId}", command.Id);
                return new UpdateImpedimentResult { Success = false };
            }

            logger.LogInformation("Updating impediment entity with Id: {ImpedimentId}", command.Id);
            Domain.Entities.Impediment.Update(item, command.Name);

            logger.LogInformation("Updating entity in repository.");
            var success = await dbContext.SaveChangesAsync(cancellationToken);

            logger.LogInformation("Update result: {Success}", success);
            logger.LogInformation("Service completed successfully.");

            return new UpdateImpedimentResult { Success = success };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while processing the command.");
            throw;
        }
    }
}