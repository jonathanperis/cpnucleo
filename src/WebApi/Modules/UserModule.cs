namespace WebApi.Modules;

public static class UserModule
{
    public static void MapUserEndpoints(this IVersionedEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/users")
            .WithTags("Users")
            .RequireAuthorization()
            .HasApiVersion(1.0);
        
        group.MapGet("/{id:guid}", GetIUserById)
            .Produces<UserDto>()
            .Produces(404)
            .WithName(nameof(GetIUserById))
            .MapToApiVersion(1.0);
        
        group.MapGet("/", GetAllUsers)
            .Produces<PaginatedResult<UserDto>>()
            .Produces(404)
            .WithName(nameof(GetAllUsers))
            .WithOpenApi(operation => 
            {
                operation.Parameters[0].Description = "Page number (default: 1)";
                operation.Parameters[1].Description = "Items per page (default: 10, max: 100)";
                operation.Parameters[2].Description = "Sort column (default: Id)";
                operation.Parameters[3].Description = "Sort order (ASC/DESC, default: ASC)";
                return operation;
            })
            .MapToApiVersion(1.0);
        
        group.MapPost("/", CreateUser)
            .Accepts<CreateUserCommand>("application/json")
            //.Produces<UserDto>(201)
            .Produces(201)
            .Produces(400)
            .MapToApiVersion(1.0)
            .WithName(nameof(CreateUser));
        
        group.MapPatch("/{id:guid}", UpdateUser)
            .Accepts<UpdateUserCommand>("application/json")
            .Produces(204)
            .Produces(400)
            .Produces(404)
            .MapToApiVersion(1.0)
            .WithName(nameof(UpdateUser));
        
        group.MapDelete("/{id:guid}", DeleteUser)
            .Produces(204)
            .MapToApiVersion(1.0)
            .WithName(nameof(DeleteUser));
    }
    
    private static async Task<IResult> GetIUserById(Guid id, ISender sender)
    {
        var response = await sender.Send(new GetUserByIdQuery(id));

        return response.OperationResult switch
        {
            OperationResult.Failed => Results.Problem(),
            OperationResult.NotFound => Results.NotFound(),
            _ => Results.Ok(response.User),
        };
    }
    
    private static async Task<IResult> GetAllUsers([AsParameters] PaginationParams paginationParams, ISender sender, HttpContext httpContext)
    {
        var response = await sender.Send(new ListUserQuery(paginationParams));
        
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
    
    private static async Task<IResult> CreateUser(CreateUserCommand command, ISender sender)
    {
        var response = await sender.Send(command);

        return response switch
        {
            OperationResult.Failed => Results.Problem(),
            OperationResult.NotFound => Results.NotFound(),
            _ => Results.Created(),
        };
    }
    
    private static async Task<IResult> UpdateUser(Guid id, UpdateUserCommand command, ISender sender)
    {
        var response = await sender.Send(command);

        return response switch
        {
            OperationResult.Failed => Results.Problem(),
            OperationResult.NotFound => Results.NotFound(),
            _ => Results.NoContent(),
        };
    }
    
    private static async Task<IResult> DeleteUser(Guid id, ISender sender)
    {
        var response = await sender.Send(new RemoveUserCommand(id));

        return response switch
        {
            OperationResult.Failed => Results.Problem(),
            OperationResult.NotFound => Results.NotFound(),
            _ => Results.NoContent(),
        };
    }
}
