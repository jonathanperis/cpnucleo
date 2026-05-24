namespace WebApi.Endpoints.Project.CreateProject;

public class Endpoint(CreateProjectHandler handler) : Endpoint<Request, Response>
{
    public override void Configure()
    {
        Post("/project");
        Description(x => x.WithTags("Projects"));

        Summary(s =>
        {
            s.Summary = "Create a new project";
            s.Description = "Creates a new project record with the given data and custom Id. Validates uniqueness and returns the created project's data.";
        });
    }

    public override async Task HandleAsync(Request request, CancellationToken cancellationToken)
    {
        var result = await handler.ExecuteAsync(
            new CreateProjectRequest(request.Id, request.Name, request.OrganizationId),
            cancellationToken);

        if (!result.Success)
        {
            AddError(r => r.Id, result.Message);
            ThrowIfAnyErrors();
        }

        Response.Project = result.Project is null
            ? null
            : new ProjectDto
            {
                Id = result.Project.Id,
                CreatedAt = result.Project.CreatedAt,
                Name = result.Project.Name,
                OrganizationId = result.Project.OrganizationId
            };

        await Send.OkAsync(Response, cancellationToken);
    }
}
