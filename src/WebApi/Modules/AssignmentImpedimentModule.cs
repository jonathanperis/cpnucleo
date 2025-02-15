namespace WebApi.Modules;

public static class AssignmentImpedimentModule
{
    public static void MapAssignmentImpedimentEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/api/assignment-impediments")
            .WithTags("AssignmentImpediments");

        group.MapGet("/", async (ISender sender) =>
        {
            var result = await sender.Send(new ListAssignmentImpedimentQuery());

            return result.OperationResult switch
            {
                OperationResult.Failed => Results.Problem(),
                OperationResult.NotFound => Results.NotFound(),
                _ => Results.Ok(result.AssignmentImpediments),
            };
        });

        group.MapGet("/{id}", async (Ulid id, ISender sender) =>
        {
            var result = await sender.Send(new GetAssignmentImpedimentByIdQuery(id));

            return result.OperationResult switch
            {
                OperationResult.Failed => Results.Problem(),
                OperationResult.NotFound => Results.NotFound(),
                _ => Results.Ok(result.AssignmentImpediment),
            };
        });

        group.MapPost("/", async (CreateAssignmentImpedimentCommand command, ISender sender) =>
        {
            var result = await sender.Send(command);

            return result switch
            {
                OperationResult.Failed => Results.Problem(),
                OperationResult.NotFound => Results.NotFound(),
                _ => Results.Created(),
            };
        });

        group.MapPut("/{id}", async (Ulid id, UpdateAssignmentImpedimentCommand command, ISender sender) =>
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
            var result = await sender.Send(new RemoveAssignmentImpedimentCommand(id));

            return result switch
            {
                OperationResult.Failed => Results.Problem(),
                OperationResult.NotFound => Results.NotFound(),
                _ => Results.NoContent(),
            };
        });
    }
}