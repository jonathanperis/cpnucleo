namespace WebApi.Modules;

public static class AssignmentImpedimentModule
{
    public static void MapAssignmentImpedimentEndpoints(this IVersionedEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/api/assignment-impediments")
            .WithTags("AssignmentImpediments")
            .HasApiVersion(1.0)
            .RequireAuthorization();
        
        group.MapGet("/", async (ISender sender) =>
        {
            var result = await sender.Send(new ListAssignmentImpedimentQuery());

            return result.OperationResult switch
            {
                OperationResult.Failed => Results.Problem(),
                OperationResult.NotFound => Results.NotFound(),
                _ => Results.Ok(result.AssignmentImpediments),
            };
        })
        .Produces<IEnumerable<AssignmentImpedimentDto>>()
        .Produces(404)
        .MapToApiVersion(1.0);

        group.MapGet("/{id:guid}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new GetAssignmentImpedimentByIdQuery(id));

            return result.OperationResult switch
            {
                OperationResult.Failed => Results.Problem(),
                OperationResult.NotFound => Results.NotFound(),
                _ => Results.Ok(result.AssignmentImpediment),
            };
        })
        .Produces<AssignmentImpedimentDto>()
        .Produces(404)
        .MapToApiVersion(1.0);

        group.MapPost("/", async (CreateAssignmentImpedimentCommand command, ISender sender) =>
        {
            var result = await sender.Send(command);

            return result switch
            {
                OperationResult.Failed => Results.Problem(),
                OperationResult.NotFound => Results.NotFound(),
                _ => Results.Created(),
            };
        })
        .Accepts<CreateAssignmentImpedimentCommand>("application/json")
        //.Produces<AssignmentImpedimentDto>(201)
        .Produces(201)
        .Produces(400)
        .MapToApiVersion(1.0);

        group.MapPatch("/{id:guid}", async (Guid id, UpdateAssignmentImpedimentCommand command, ISender sender) =>
        {
            var result = await sender.Send(command);

            return result switch
            {
                OperationResult.Failed => Results.Problem(),
                OperationResult.NotFound => Results.NotFound(),
                _ => Results.NoContent(),
            };
        })
        .Accepts<UpdateAssignmentImpedimentCommand>("application/json")
        .Produces(204)
        .Produces(400)
        .Produces(404)
        .MapToApiVersion(1.0);

        group.MapDelete("/{id:guid}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new RemoveAssignmentImpedimentCommand(id));

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