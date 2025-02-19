namespace WebApi.Modules;

public static class UserAssignmentModule
{
    public static void MapUserAssignmentEndpoints(this IVersionedEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/api/user-assignments")
            .WithTags("UserAssignments")
            .RequireAuthorization()
            .HasApiVersion(1.0);

        group.MapGet("/", async (ISender sender) =>
        {
            var result = await sender.Send(new ListUserAssignmentsQuery());

            return result.OperationResult switch
            {
                OperationResult.Failed => Results.Problem(),
                OperationResult.NotFound => Results.NotFound(),
                _ => Results.Ok(result.UserAssignments),
            };
        })
        .Produces<IEnumerable<UserAssignmentDto>>()
        .Produces(404)
        .MapToApiVersion(1.0);

        group.MapGet("/{id:guid}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new GetUserAssignmentByIdQuery(id));

            return result.OperationResult switch
            {
                OperationResult.Failed => Results.Problem(),
                OperationResult.NotFound => Results.NotFound(),
                _ => Results.Ok(result.UserAssignment),
            };
        })
        .Produces<UserAssignmentDto>()
        .Produces(404)
        .MapToApiVersion(1.0);

        group.MapPost("/", async (CreateUserAssignmentCommand command, ISender sender) =>
        {
            var result = await sender.Send(command);

            return result switch
            {
                OperationResult.Failed => Results.Problem(),
                OperationResult.NotFound => Results.NotFound(),
                _ => Results.Created(),
            };
        })
        .Accepts<CreateUserAssignmentCommand>("application/json")
        //.Produces<UserAssignmentDto>(201)
        .Produces(201)
        .Produces(400)
        .MapToApiVersion(1.0);

        group.MapPatch("/{id:guid}", async (Guid id, UpdateUserAssignmentCommand command, ISender sender) =>
        {
            var result = await sender.Send(command);

            return result switch
            {
                OperationResult.Failed => Results.Problem(),
                OperationResult.NotFound => Results.NotFound(),
                _ => Results.NoContent(),
            };
        })
        .Accepts<UpdateUserAssignmentCommand>("application/json")
        .Produces(204)
        .Produces(400)
        .Produces(404)
        .MapToApiVersion(1.0);

        group.MapDelete("/{id:guid}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new RemoveUserAssignmentCommand(id));

            return result switch
            {
                OperationResult.Failed => Results.Problem(),
                OperationResult.NotFound => Results.NotFound(),
                _ => Results.NoContent(),
            };
        })
        .Produces(204)
        .MapToApiVersion(1.0);
    }
}
