namespace WebApi.Modules;

public static class ProjectModule
{
    public static void MapProjectEndpoints(this IVersionedEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/projects")
            .WithTags("Projects")
            .RequireAuthorization()
            .HasApiVersion(1.0);
        
        group.MapGet("/{id:guid}", GetIProjectById)
            .Produces<ProjectDto>()
            .Produces(404)
            .WithName(nameof(GetIProjectById))
            .MapToApiVersion(1.0);
        
        group.MapGet("/", GetAllProjects)
            .Produces<PaginatedResult<ProjectDto>>()
            .Produces(404)
            .WithName(nameof(GetAllProjects))
            .WithOpenApi(operation => 
            {
                operation.Parameters[0].Description = "Page number (default: 1)";
                operation.Parameters[1].Description = "Items per page (default: 10, max: 100)";
                operation.Parameters[2].Description = "Sort column (default: Id)";
                operation.Parameters[3].Description = "Sort order (ASC/DESC, default: ASC)";
                return operation;
            })
            .MapToApiVersion(1.0);
        
        group.MapPost("/", CreateProject)
            .Accepts<CreateProjectCommand>("application/json")
            //.Produces<ProjectDto>(201)
            .Produces(201)
            .Produces(400)
            .MapToApiVersion(1.0)
            .WithName(nameof(CreateProject));
        
        group.MapPatch("/{id:guid}", UpdateProject)
            .Accepts<UpdateProjectCommand>("application/json")
            .Produces(204)
            .Produces(400)
            .Produces(404)
            .MapToApiVersion(1.0)
            .WithName(nameof(UpdateProject));
        
        group.MapDelete("/{id:guid}", DeleteProject)
            .Produces(204)
            .MapToApiVersion(1.0)
            .WithName(nameof(DeleteProject));
    }
    
    private static async Task<IResult> GetIProjectById(Guid id, ISender sender)
    {
        var response = await sender.Send(new GetProjectByIdQuery(id));

        return response.OperationResult switch
        {
            OperationResult.Failed => Results.Problem(),
            OperationResult.NotFound => Results.NotFound(),
            _ => Results.Ok(response.Project),
        };
    }
    
    private static async Task<IResult> GetAllProjects([AsParameters] PaginationParams paginationParams, ISender sender, HttpContext httpContext)
    {
        var response = await sender.Send(new ListProjectQuery(paginationParams));
        
        httpContext.Response.Headers.Append("X-Pagination-TotalCount", response.Result.TotalCount.ToString());
        httpContext.Response.Headers.Append("X-Pagination-PageSize", response.Result.PageSize.ToString());
        httpContext.Response.Headers.Append("X-Pagination-CurrentPage", response.Result.PageNumber.ToString());
        httpContext.Response.Headers.Append("X-Pagination-TotalPages", response.Result.TotalPages.ToString());
        
        return response.OperationResult switch
        {
            OperationResult.Failed => Results.Problem(),
            OperationResult.NotFound => Results.NotFound(),
            _ => Results.Ok(response.Result),
        };
    }
    
    private static async Task<IResult> CreateProject(CreateProjectCommand command, ISender sender)
    {
        var response = await sender.Send(command);

        return response switch
        {
            OperationResult.Failed => Results.Problem(),
            OperationResult.NotFound => Results.NotFound(),
            _ => Results.Created(),
        };
    }
    
    private static async Task<IResult> UpdateProject(Guid id, UpdateProjectCommand command, ISender sender)
    {
        var response = await sender.Send(command);

        return response switch
        {
            OperationResult.Failed => Results.Problem(),
            OperationResult.NotFound => Results.NotFound(),
            _ => Results.NoContent(),
        };
    }
    
    private static async Task<IResult> DeleteProject(Guid id, ISender sender)
    {
        var response = await sender.Send(new RemoveProjectCommand(id));

        return response switch
        {
            OperationResult.Failed => Results.Problem(),
            OperationResult.NotFound => Results.NotFound(),
            _ => Results.NoContent(),
        };
    }
}
