namespace WebApi.Modules;

public static class UserModule
{
    public static void MapUserEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/api/users");

        group.MapGet("/", async (ISender sender) => 
        {
            var result = await sender.Send(new ListUserQuery());

            return result.OperationResult switch
            {
                OperationResult.Failed => Results.BadRequest(),
                OperationResult.NotFound => Results.NotFound(),
                _ => Results.Ok(result.Users),
            };
        })
        .WithTags("Users");

        group.MapGet("/{id}", async (Ulid id, ISender sender) => 
        {
            var result = await sender.Send(new GetUserByIdQuery(id));

            return result.OperationResult switch
            {
                OperationResult.Failed => Results.BadRequest(),
                OperationResult.NotFound => Results.NotFound(),
                _ => Results.Ok(result.User),
            };
        })
        .WithTags("Users");

        group.MapPost("/", async (CreateUserCommand command, ISender sender) => 
        {
            var result = await sender.Send(command);

            return result switch
            {
                OperationResult.Failed => Results.BadRequest(),
                OperationResult.NotFound => Results.NotFound(),
                _ => Results.Created(),
            };
        })
        .WithTags("Users");

        group.MapPut("/{id}", async (Ulid id, UpdateUserCommand command, ISender sender) => 
        {
            var result = await sender.Send(command);

            return result switch
            {
                OperationResult.Failed => Results.BadRequest(),
                OperationResult.NotFound => Results.NotFound(),
                _ => Results.NoContent(),
            };
        })
        .WithTags("Users");

        group.MapDelete("/{id}", async (Ulid id, ISender sender) => 
        {
            var result = await sender.Send(new RemoveUserCommand(id));

            return result switch
            {
                OperationResult.Failed => Results.BadRequest(),
                OperationResult.NotFound => Results.NotFound(),
                _ => Results.NoContent(),
            };
        })
        .WithTags("Users");
    }
}