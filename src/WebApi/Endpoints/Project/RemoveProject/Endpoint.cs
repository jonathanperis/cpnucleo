namespace WebApi.Endpoints.Project.RemoveProject;

// Dapper Repository Basic
public class Endpoint(IProjectRepository repository) : Endpoint<Request, Response>
{
    public override void Configure()
    {
        Delete("/api/project");
        Description(x => x.WithTags("Projects"));
        AllowAnonymous();

        Summary(s => {
            s.Summary = "Delete projects by Ids";
            s.Description = "Deletes the projects specified by the provided Ids. Validates existence of each, removes them, updates the repository, and commits the transaction.";
        });   
    }

    public override async Task HandleAsync(Request request, CancellationToken cancellationToken)
    {        
        Logger.LogInformation("Service started processing request.");
        
        try
        {
            Logger.LogInformation("Checking if project entities exist for Ids: {ProjectIds}", string.Join(",", request.Ids));
            var allSuccess = true;

            foreach (var id in request.Ids)
            {
                var item = await repository.GetByIdAsync(id);
                if (item is null)
                {
                    await SendNotFoundAsync(cancellation: cancellationToken);
                    return;
                }

                Logger.LogInformation("Removing project entity with Id: {ProjectId}", id);
                Domain.Entities.Project.Remove(item);

                Logger.LogInformation("Updating repository for removed entity {ProjectId}.", id);
                var result = await repository.UpdateAsync(item);

                if (!result) allSuccess = false;
            }
            
            Response.Success = allSuccess;
            
            if (!allSuccess)
            {
                Logger.LogWarning("One or more deletions failed.");
                await SendErrorsAsync(cancellation: cancellationToken);
                return;
            }

            Logger.LogInformation("Remove result: {Success}", Response.Success);          
            Logger.LogInformation("Service completed successfully.");
            
            await SendOkAsync(Response, cancellation: cancellationToken);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "An error occurred while processing the request.");
            ThrowError("An error has occurred.");
        }
    }
}