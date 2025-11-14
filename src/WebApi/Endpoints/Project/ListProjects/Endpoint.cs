using WebApi.Common.Extensions;

namespace WebApi.Endpoints.Project.ListProjects;

// Dapper Repository Basic
public class Endpoint(IProjectRepository repository) : Endpoint<Request, Response>
{
    public override void Configure()
    {
        Get("/api/projects");
        Description(x => x.WithTags("Projects"));
        AllowAnonymous();

        Summary(s =>
        {
            s.Summary = "Retrieve a paginated list of projects";
            s.Description = "Fetches projects based on pagination parameters, maps entities to DTOs, and returns paginated results with metadata (total count, page number, and page size).";
        });
    }

    public override async Task HandleAsync(Request request, CancellationToken cancellationToken)
    {
        Logger.LogInformation("Service started processing request.");
        Logger.LogInformation("Fetching all projects with pagination page {PageNumber}, size {PageSize}", request.Pagination.PageNumber, request.Pagination.PageSize);

        var response = await repository.GetAllAsync(request.Pagination);

        Logger.LogInformation("Fetched {Count} project records", response.Data?.Count() ?? 0);
        Logger.LogInformation("Mapping entities to DTOs.");

        Response.Result = response.MapToDto(x => x?.MapToDto());

        Logger.LogInformation("Mapping complete, setting response result.");
        Logger.LogInformation("Service completed successfully.");

        await Send.OkAsync(Response, cancellationToken);
    }
}
