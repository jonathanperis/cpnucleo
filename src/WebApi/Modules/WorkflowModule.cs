namespace WebApi.Modules;

public static class WorkflowModule
{
    public static void MapWorkflowEndpoints(this IVersionedEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/workflows")
            .WithTags("Workflows")
            .RequireAuthorization()
            .HasApiVersion(1.0);
        
        group.MapGet("/{id:guid}", GetIWorkflowById)
            .Produces<WorkflowDto>()
            .Produces(404)
            .WithName(nameof(GetIWorkflowById))
            .MapToApiVersion(1.0);
        
        group.MapGet("/", GetAllWorkflows)
            .Produces<PaginatedResult<WorkflowDto>>()
            .Produces(404)
            .WithName(nameof(GetAllWorkflows))
            .WithOpenApi(operation => 
            {
                operation.Parameters[0].Description = "Page number (default: 1)";
                operation.Parameters[1].Description = "Items per page (default: 10, max: 100)";
                operation.Parameters[2].Description = "Sort column (default: Id)";
                operation.Parameters[3].Description = "Sort order (ASC/DESC, default: ASC)";
                return operation;
            })
            .MapToApiVersion(1.0);
        
        group.MapPost("/", CreateWorkflow)
            .Accepts<CreateWorkflowCommand>("application/json")
            //.Produces<WorkflowDto>(201)
            .Produces(201)
            .Produces(400)
            .MapToApiVersion(1.0)
            .WithName(nameof(CreateWorkflow));
        
        group.MapPatch("/{id:guid}", UpdateWorkflow)
            .Accepts<UpdateWorkflowCommand>("application/json")
            .Produces(204)
            .Produces(400)
            .Produces(404)
            .MapToApiVersion(1.0)
            .WithName(nameof(UpdateWorkflow));
        
        group.MapDelete("/{id:guid}", DeleteWorkflow)
            .Produces(204)
            .MapToApiVersion(1.0)
            .WithName(nameof(DeleteWorkflow));
    }
    
    private static async Task<IResult> GetIWorkflowById(Guid id, ISender sender)
    {
        var response = await sender.Send(new GetWorkflowByIdQuery(id));

        return response.OperationResult switch
        {
            OperationResult.Failed => Results.Problem(),
            OperationResult.NotFound => Results.NotFound(),
            _ => Results.Ok(response.Workflow),
        };
    }
    
    private static async Task<IResult> GetAllWorkflows([AsParameters] PaginationParams paginationParams, ISender sender, HttpContext httpContext)
    {
        var response = await sender.Send(new ListWorkflowQuery(paginationParams));
        
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
    
    private static async Task<IResult> CreateWorkflow(CreateWorkflowCommand command, ISender sender)
    {
        var response = await sender.Send(command);

        return response switch
        {
            OperationResult.Failed => Results.Problem(),
            OperationResult.NotFound => Results.NotFound(),
            _ => Results.Created(),
        };
    }
    
    private static async Task<IResult> UpdateWorkflow(Guid id, UpdateWorkflowCommand command, ISender sender)
    {
        var response = await sender.Send(command);

        return response switch
        {
            OperationResult.Failed => Results.Problem(),
            OperationResult.NotFound => Results.NotFound(),
            _ => Results.NoContent(),
        };
    }
    
    private static async Task<IResult> DeleteWorkflow(Guid id, ISender sender)
    {
        var response = await sender.Send(new RemoveWorkflowCommand(id));

        return response switch
        {
            OperationResult.Failed => Results.Problem(),
            OperationResult.NotFound => Results.NotFound(),
            _ => Results.NoContent(),
        };
    }
}
