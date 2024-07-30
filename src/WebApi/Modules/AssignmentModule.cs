namespace WebApi.Modules;

public static class AssignmentModule
{
    public static void MapAssignmentEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/api/assignments")
            .WithTags("Assignments");

        group.MapGet("/", async (ISender sender) =>
        {
            var result = await sender.Send(new ListAssignmentQuery());

            return result.OperationResult switch
            {
                OperationResult.Failed => Results.Problem(),
                OperationResult.NotFound => Results.NotFound(),
                _ => Results.Ok(result.Assignments),
            };
        });

        group.MapGet("/{id}", async (Ulid id, ISender sender) =>
        {
            var result = await sender.Send(new GetAssignmentByIdQuery(id));

            return result.OperationResult switch
            {
                OperationResult.Failed => Results.Problem(),
                OperationResult.NotFound => Results.NotFound(),
                _ => Results.Ok(result.Assignment),
            };
        });

        group.MapPost("/", async (CreateAssignmentCommand command, ISender sender) =>
        {
            var result = await sender.Send(command);

            return result switch
            {
                OperationResult.Failed => Results.Problem(),
                OperationResult.NotFound => Results.NotFound(),
                _ => Results.Created(),
            };
        });

        group.MapPut("/{id}", async (Ulid id, UpdateAssignmentCommand command, ISender sender) =>
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
            var result = await sender.Send(new RemoveAssignmentCommand(id));

            return result switch
            {
                OperationResult.Failed => Results.Problem(),
                OperationResult.NotFound => Results.NotFound(),
                _ => Results.NoContent(),
            };
        });
    }
}
