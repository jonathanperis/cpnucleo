namespace WebApi.Endpoints.AssignmentImpediment.ListAssignmentImpediments;

// Dapper Repository Advanced
public class Endpoint(IUnitOfWork unitOfWork) : Endpoint<Request, Response>
{
    public override void Configure()
    {
        Get("/assignmentImpediments");
        Description(x => x.WithTags("AssignmentImpediments"));

        Summary(s =>
        {
            s.Summary = "Retrieve a paginated list of assignmentImpediments";
            s.Description = "Fetches assignmentImpediments based on pagination parameters, maps entities to DTOs, and returns paginated results with metadata (total count, page number, and page size).";
        });
    }

    public override async Task HandleAsync(Request request, CancellationToken cancellationToken)
    {
        if (HttpContext.Request.AcceptsServerSentEvents())
        {
            await TypedResults
                .ServerSentEvents(ListingSseExtensions.CreateListingStream(ct => BuildResponseAsync(request, ct), Logger, cancellationToken), "listing")
                .ExecuteAsync(HttpContext);
            return;
        }

        var response = await BuildResponseAsync(request, cancellationToken);
        await Send.OkAsync(response, cancellationToken);
    }

    private async Task<Response> BuildResponseAsync(Request request, CancellationToken cancellationToken)
    {
        Logger.LogInformation("Service started processing request.");
        Logger.LogInformation("Fetching all assignmentImpediments with pagination page {PageNumber}, size {PageSize}", request.Pagination.PageNumber, request.Pagination.PageSize);

        var repository = unitOfWork.GetRepository<Domain.Entities.AssignmentImpediment>();
        var response = await repository.GetAllAsync(request.Pagination);

        Logger.LogInformation("Fetched {Count} assignmentImpediment records", response.Data?.Count() ?? 0);
        Logger.LogInformation("Mapping entities to DTOs.");


        Logger.LogInformation("Mapping complete, setting response result.");
        Logger.LogInformation("Service completed successfully.");

        return new Response { Result = response.MapToDto(x => x?.MapToDto()) };
    }
}
