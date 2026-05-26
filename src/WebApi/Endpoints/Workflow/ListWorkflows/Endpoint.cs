namespace WebApi.Endpoints.Workflow.ListWorkflows;

// Dapper Repository Advanced
public class Endpoint(IUnitOfWork unitOfWork) : Endpoint<Request, Response>
{
    public override void Configure()
    {
        Get("/workflows");
        Description(x => x.WithTags("Workflows"));

        Summary(s =>
        {
            s.Summary = "Retrieve a paginated list of workflows";
            s.Description = "Fetches workflows based on pagination parameters, maps entities to DTOs, and returns paginated results with metadata (total count, page number, and page size).";
        });
    }

    public override async Task HandleAsync(Request request, CancellationToken cancellationToken)
    {
        if (HttpContext.Request.AcceptsServerSentEvents())
        {
            await TypedResults
                .ServerSentEvents(ListingSseExtensions.CreateListingStream(ct => BuildResponseAsync(request, ct), HttpContext.RequestServices.GetRequiredService<ListingChangeNotifier>(), Logger, cancellationToken), "listing")
                .ExecuteAsync(HttpContext);
            return;
        }

        var response = await BuildResponseAsync(request, cancellationToken);
        await Send.OkAsync(response, cancellationToken);
    }

    private async Task<Response> BuildResponseAsync(Request request, CancellationToken cancellationToken)
    {
        Logger.LogInformation("Service started processing request.");
        Logger.LogInformation("Fetching all workflows with pagination page {PageNumber}, size {PageSize}", request.Pagination.PageNumber, request.Pagination.PageSize);

        var repository = unitOfWork.GetRepository<Domain.Entities.Workflow>();
        var response = await repository.GetAllAsync(request.Pagination, cancellationToken);

        Logger.LogInformation("Fetched {Count} workflow records", response.Data?.Count() ?? 0);
        Logger.LogInformation("Mapping entities to DTOs.");


        Logger.LogInformation("Mapping complete, setting response result.");
        Logger.LogInformation("Service completed successfully.");

        return new Response { Result = response.MapToDto(x => x?.MapToDto()) };
    }
}
