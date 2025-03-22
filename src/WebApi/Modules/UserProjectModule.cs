namespace WebApi.Modules;

public static class UserProjectModule
{
    public static void MapUserProjectEndpoints(this IVersionedEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/userProjects")
            .WithTags("UserProjects")
            .RequireAuthorization()
            .HasApiVersion(1.0);
        
        group.MapGet("/{id:guid}", GetIUserProjectById)
            .Produces<UserProjectDto>()
            .Produces(404)
            .WithName(nameof(GetIUserProjectById))
            .MapToApiVersion(1.0);
        
        group.MapGet("/", GetAllUserProjects)
            .Produces<PaginatedResult<UserProjectDto>>()
            .Produces(404)
            .WithName(nameof(GetAllUserProjects))
            .WithOpenApi(operation => 
            {
                operation.Parameters[0].Description = "Page number (default: 1)";
                operation.Parameters[1].Description = "Items per page (default: 10, max: 100)";
                operation.Parameters[2].Description = "Sort column (default: Id)";
                operation.Parameters[3].Description = "Sort order (ASC/DESC, default: ASC)";
                return operation;
            })
            .MapToApiVersion(1.0);
        
        group.MapPost("/", CreateUserProject)
            .Accepts<CreateUserProjectCommand>("application/json")
            //.Produces<UserProjectDto>(201)
            .Produces(201)
            .Produces(400)
            .MapToApiVersion(1.0)
            .WithName(nameof(CreateUserProject));
        
        group.MapPatch("/{id:guid}", UpdateUserProject)
            .Accepts<UpdateUserProjectCommand>("application/json")
            .Produces(204)
            .Produces(400)
            .Produces(404)
            .MapToApiVersion(1.0)
            .WithName(nameof(UpdateUserProject));
        
        group.MapDelete("/{id:guid}", DeleteUserProject)
            .Produces(204)
            .MapToApiVersion(1.0)
            .WithName(nameof(DeleteUserProject));
    }
    
    private static async Task<IResult> GetIUserProjectById(Guid id, ISender sender)
    {
        var response = await sender.Send(new GetUserProjectByIdQuery(id));

        return response.OperationResult switch
        {
            OperationResult.Failed => Results.Problem(),
            OperationResult.NotFound => Results.NotFound(),
            _ => Results.Ok(response.UserProject),
        };
    }
    
    private static async Task<IResult> GetAllUserProjects([AsParameters] PaginationParams paginationParams, ISender sender, HttpContext httpContext)
    {
        var response = await sender.Send(new ListUserProjectQuery(paginationParams));
        
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
    
    private static async Task<IResult> CreateUserProject(CreateUserProjectCommand command, ISender sender)
    {
        var response = await sender.Send(command);

        return response switch
        {
            OperationResult.Failed => Results.Problem(),
            OperationResult.NotFound => Results.NotFound(),
            _ => Results.Created(),
        };
    }
    
    private static async Task<IResult> UpdateUserProject(Guid id, UpdateUserProjectCommand command, ISender sender)
    {
        var response = await sender.Send(command);

        return response switch
        {
            OperationResult.Failed => Results.Problem(),
            OperationResult.NotFound => Results.NotFound(),
            _ => Results.NoContent(),
        };
    }
    
    private static async Task<IResult> DeleteUserProject(Guid id, ISender sender)
    {
        var response = await sender.Send(new RemoveUserProjectCommand(id));

        return response switch
        {
            OperationResult.Failed => Results.Problem(),
            OperationResult.NotFound => Results.NotFound(),
            _ => Results.NoContent(),
        };
    }
}
