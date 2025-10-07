namespace WebApi.Endpoints.Workflow.GetWorkflowById;

// Dapper Repository Advanced
public class Endpoint(IUnitOfWork unitOfWork) : Endpoint<Request, Response>
{
    public override void Configure()
    {
        Get("/api/workflow");
        Description(x => x.WithTags("Workflows"));
        AllowAnonymous();

        Summary(s =>
        {
            s.Summary = "Retrieve an workflow by Id";
            s.Description = "Fetches the workflow matching the provided Id. Returns 404 if not found, otherwise returns the workflow data mapped to a DTO.";
        });
    }

    public override async Task HandleAsync(Request request, CancellationToken cancellationToken)
    {
        Logger.LogInformation("Service started processing request.");

        Logger.LogInformation("Fetching workflow entity with Id: {WorkflowId}", request.Id);
        var repository = unitOfWork.GetRepository<Domain.Entities.Workflow>();
        var item = await repository.GetByIdAsync(request.Id);

        if (item is null)
        {
            Logger.LogWarning("Workflow not found with Id: {WorkflowId}", request.Id);
            await Send.NotFoundAsync(cancellation: cancellationToken);
            return;
        }

        Logger.LogInformation("Mapping entity to DTO and setting response for Id: {WorkflowId}", request.Id);
        Response.Workflow = item.MapToDto();

        Logger.LogInformation("Service completed successfully.");

        await Send.OkAsync(Response, cancellationToken);
    }
}
