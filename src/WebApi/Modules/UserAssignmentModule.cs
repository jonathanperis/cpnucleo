namespace WebApi.Modules;

public static class UserAssignmentModule
{
    public static void MapUserAssignmentEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/api/user-assignments")
            .WithTags("UserAssignments");

        group.MapGet("/", async (ISender sender) =>
        {
            var result = await sender.Send(new ListUserAssignmentsQuery());

            return result.OperationResult switch
            {
                OperationResult.Failed => Results.Problem(),
                OperationResult.NotFound => Results.NotFound(),
                _ => Results.Ok(result.UserAssignments),
            };
        });

        group.MapGet("/{id}", async (Ulid id, ISender sender) =>
        {
            var result = await sender.Send(new GetUserAssignmentByIdQuery(id));

            return result.OperationResult switch
            {
                OperationResult.Failed => Results.Problem(),
                OperationResult.NotFound => Results.NotFound(),
                _ => Results.Ok(result.UserAssignment),
            };
        });

        group.MapPost("/", async (CreateUserAssignmentCommand command, ISender sender) =>
        {
            var result = await sender.Send(command);

            return result switch
            {
                OperationResult.Failed => Results.Problem(),
                OperationResult.NotFound => Results.NotFound(),
                _ => Results.Created(),
            };
        });

        group.MapPut("/{id}", async (Ulid id, UpdateUserAssignmentCommand command, ISender sender) =>
        {
            var result = await sender.Send(command);

            return result switch
            {
                OperationResult.Failed => Results.Problem(),
                OperationResult.NotFound => Results.NotFound(),
                _ => Results.NoContent(),
            };
        });

        group.MapDelete("/{id}", async (Ulid id, ISender sender) =>
        {
            var result = await sender.Send(new RemoveUserAssignmentCommand(id));

            return result switch
            {
                OperationResult.Failed => Results.Problem(),
                OperationResult.NotFound => Results.NotFound(),
                _ => Results.NoContent(),
            };
        });
    }
}
