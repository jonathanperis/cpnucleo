namespace WebApi.Modules;

public static class AssignmentTypeModule
{
    public static void MapAssignmentTypeEndpoints(this IVersionedEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/assignmentTypes")
            .WithTags("AssignmentTypes")
            .RequireAuthorization()
            .HasApiVersion(1.0);
        
        group.MapGet("/{id:guid}", GetIAssignmentTypeById)
            .Produces<AssignmentTypeDto>()
            .Produces(404)
            .WithName(nameof(GetIAssignmentTypeById))
            .MapToApiVersion(1.0);
        
        group.MapGet("/", GetAllAssignmentTypes)
            .Produces<PaginatedResult<AssignmentTypeDto>>()
            .Produces(404)
            .WithName(nameof(GetAllAssignmentTypes))
            .WithOpenApi(operation => 
            {
                operation.Parameters[0].Description = "Page number (default: 1)";
                operation.Parameters[1].Description = "Items per page (default: 10, max: 100)";
                operation.Parameters[2].Description = "Sort column (default: Id)";
                operation.Parameters[3].Description = "Sort order (ASC/DESC, default: ASC)";
                return operation;
            })
            .MapToApiVersion(1.0);
        
        group.MapPost("/", CreateAssignmentType)
            .Accepts<CreateAssignmentTypeCommand>("application/json")
            //.Produces<AssignmentTypeDto>(201)
            .Produces(201)
            .Produces(400)
            .MapToApiVersion(1.0)
            .WithName(nameof(CreateAssignmentType));
        
        group.MapPatch("/{id:guid}", UpdateAssignmentType)
            .Accepts<UpdateAssignmentTypeCommand>("application/json")
            .Produces(204)
            .Produces(400)
            .Produces(404)
            .MapToApiVersion(1.0)
            .WithName(nameof(UpdateAssignmentType));
        
        group.MapDelete("/{id:guid}", DeleteAssignmentType)
            .Produces(204)
            .MapToApiVersion(1.0)
            .WithName(nameof(DeleteAssignmentType));
    }
    
    private static async Task<IResult> GetIAssignmentTypeById(Guid id, ISender sender)
    {
        var response = await sender.Send(new GetAssignmentTypeByIdQuery(id));

        return response.OperationResult switch
        {
            OperationResult.Failed => Results.Problem(),
            OperationResult.NotFound => Results.NotFound(),
            _ => Results.Ok(response.AssignmentType),
        };
    }
    
    private static async Task<IResult> GetAllAssignmentTypes([AsParameters] PaginationParams paginationParams, ISender sender, HttpContext httpContext)
    {
        var response = await sender.Send(new ListAssignmentTypeQuery(paginationParams));
        
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
    
    private static async Task<IResult> CreateAssignmentType(CreateAssignmentTypeCommand command, ISender sender)
    {
        var response = await sender.Send(command);

        return response switch
        {
            OperationResult.Failed => Results.Problem(),
            OperationResult.NotFound => Results.NotFound(),
            _ => Results.Created(),
        };
    }
    
    private static async Task<IResult> UpdateAssignmentType(Guid id, UpdateAssignmentTypeCommand command, ISender sender)
    {
        var response = await sender.Send(command);

        return response switch
        {
            OperationResult.Failed => Results.Problem(),
            OperationResult.NotFound => Results.NotFound(),
            _ => Results.NoContent(),
        };
    }
    
    private static async Task<IResult> DeleteAssignmentType(Guid id, ISender sender)
    {
        var response = await sender.Send(new RemoveAssignmentTypeCommand(id));

        return response switch
        {
            OperationResult.Failed => Results.Problem(),
            OperationResult.NotFound => Results.NotFound(),
            _ => Results.NoContent(),
        };
    }
}
