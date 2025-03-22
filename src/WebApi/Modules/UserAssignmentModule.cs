namespace WebApi.Modules;

public static class UserAssignmentModule
{
    public static void MapUserAssignmentEndpoints(this IVersionedEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/userAssignments")
            .WithTags("UserAssignments")
            .RequireAuthorization()
            .HasApiVersion(1.0);
        
        group.MapGet("/{id:guid}", GetIUserAssignmentById)
            .Produces<UserAssignmentDto>()
            .Produces(404)
            .WithName(nameof(GetIUserAssignmentById))
            .MapToApiVersion(1.0);
        
        group.MapGet("/", GetAllUserAssignments)
            .Produces<PaginatedResult<UserAssignmentDto>>()
            .Produces(404)
            .WithName(nameof(GetAllUserAssignments))
            .WithOpenApi(operation => 
            {
                operation.Parameters[0].Description = "Page number (default: 1)";
                operation.Parameters[1].Description = "Items per page (default: 10, max: 100)";
                operation.Parameters[2].Description = "Sort column (default: Id)";
                operation.Parameters[3].Description = "Sort order (ASC/DESC, default: ASC)";
                return operation;
            })
            .MapToApiVersion(1.0);
        
        group.MapPost("/", CreateUserAssignment)
            .Accepts<CreateUserAssignmentCommand>("application/json")
            //.Produces<UserAssignmentDto>(201)
            .Produces(201)
            .Produces(400)
            .MapToApiVersion(1.0)
            .WithName(nameof(CreateUserAssignment));
        
        group.MapPatch("/{id:guid}", UpdateUserAssignment)
            .Accepts<UpdateUserAssignmentCommand>("application/json")
            .Produces(204)
            .Produces(400)
            .Produces(404)
            .MapToApiVersion(1.0)
            .WithName(nameof(UpdateUserAssignment));
        
        group.MapDelete("/{id:guid}", DeleteUserAssignment)
            .Produces(204)
            .MapToApiVersion(1.0)
            .WithName(nameof(DeleteUserAssignment));
    }
    
    private static async Task<IResult> GetIUserAssignmentById(Guid id, ISender sender)
    {
        var response = await sender.Send(new GetUserAssignmentByIdQuery(id));

        return response.OperationResult switch
        {
            OperationResult.Failed => Results.Problem(),
            OperationResult.NotFound => Results.NotFound(),
            _ => Results.Ok(response.UserAssignment),
        };
    }
    
    private static async Task<IResult> GetAllUserAssignments([AsParameters] PaginationParams paginationParams, ISender sender, HttpContext httpContext)
    {
        var response = await sender.Send(new ListUserAssignmentQuery(paginationParams));
        
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
    
    private static async Task<IResult> CreateUserAssignment(CreateUserAssignmentCommand command, ISender sender)
    {
        var response = await sender.Send(command);

        return response switch
        {
            OperationResult.Failed => Results.Problem(),
            OperationResult.NotFound => Results.NotFound(),
            _ => Results.Created(),
        };
    }
    
    private static async Task<IResult> UpdateUserAssignment(Guid id, UpdateUserAssignmentCommand command, ISender sender)
    {
        var response = await sender.Send(command);

        return response switch
        {
            OperationResult.Failed => Results.Problem(),
            OperationResult.NotFound => Results.NotFound(),
            _ => Results.NoContent(),
        };
    }
    
    private static async Task<IResult> DeleteUserAssignment(Guid id, ISender sender)
    {
        var response = await sender.Send(new RemoveUserAssignmentCommand(id));

        return response switch
        {
            OperationResult.Failed => Results.Problem(),
            OperationResult.NotFound => Results.NotFound(),
            _ => Results.NoContent(),
        };
    }
}
