namespace WebApi.Modules;

public static class AssignmentModule
{
    public static void MapAssignmentEndpoints(this IVersionedEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/assignments")
            .WithTags("Assignments")
            .RequireAuthorization()
            .HasApiVersion(1.0);
        
        group.MapGet("/{id:guid}", GetIAssignmentById)
            .Produces<AssignmentDto>()
            .Produces(404)
            .WithName(nameof(GetIAssignmentById))
            .MapToApiVersion(1.0);
        
        group.MapGet("/", GetAllAssignments)
            .Produces<PaginatedResult<AssignmentDto>>()
            .Produces(404)
            .WithName(nameof(GetAllAssignments))
            .WithOpenApi(operation => 
            {
                operation.Parameters[0].Description = "Page number (default: 1)";
                operation.Parameters[1].Description = "Items per page (default: 10, max: 100)";
                operation.Parameters[2].Description = "Sort column (default: Id)";
                operation.Parameters[3].Description = "Sort order (ASC/DESC, default: ASC)";
                return operation;
            })
            .MapToApiVersion(1.0);
        
        group.MapPost("/", CreateAssignment)
            .Accepts<CreateAssignmentCommand>("application/json")
            //.Produces<AssignmentDto>(201)
            .Produces(201)
            .Produces(400)
            .MapToApiVersion(1.0)
            .WithName(nameof(CreateAssignment));
        
        group.MapPatch("/{id:guid}", UpdateAssignment)
            .Accepts<UpdateAssignmentCommand>("application/json")
            .Produces(204)
            .Produces(400)
            .Produces(404)
            .MapToApiVersion(1.0)
            .WithName(nameof(UpdateAssignment));
        
        group.MapDelete("/{id:guid}", DeleteAssignment)
            .Produces(204)
            .MapToApiVersion(1.0)
            .WithName(nameof(DeleteAssignment));
    }
    
    private static async Task<IResult> GetIAssignmentById(Guid id, ISender sender)
    {
        var response = await sender.Send(new GetAssignmentByIdQuery(id));

        return response.OperationResult switch
        {
            OperationResult.Failed => Results.Problem(),
            OperationResult.NotFound => Results.NotFound(),
            _ => Results.Ok(response.Assignment),
        };
    }
    
    private static async Task<IResult> GetAllAssignments([AsParameters] PaginationParams paginationParams, ISender sender, HttpContext httpContext)
    {
        var response = await sender.Send(new ListAssignmentQuery(paginationParams));
        
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
    
    private static async Task<IResult> CreateAssignment(CreateAssignmentCommand command, ISender sender)
    {
        var response = await sender.Send(command);

        return response switch
        {
            OperationResult.Failed => Results.Problem(),
            OperationResult.NotFound => Results.NotFound(),
            _ => Results.Created(),
        };
    }
    
    private static async Task<IResult> UpdateAssignment(Guid id, UpdateAssignmentCommand command, ISender sender)
    {
        var response = await sender.Send(command);

        return response switch
        {
            OperationResult.Failed => Results.Problem(),
            OperationResult.NotFound => Results.NotFound(),
            _ => Results.NoContent(),
        };
    }
    
    private static async Task<IResult> DeleteAssignment(Guid id, ISender sender)
    {
        var response = await sender.Send(new RemoveAssignmentCommand(id));

        return response switch
        {
            OperationResult.Failed => Results.Problem(),
            OperationResult.NotFound => Results.NotFound(),
            _ => Results.NoContent(),
        };
    }
}
