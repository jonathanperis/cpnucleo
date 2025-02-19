namespace WebApi.Modules;

public static class UserProjectModule
{
    public static void MapUserProjectEndpoints(this IVersionedEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/api/user-projects")
            .WithTags("UserProjects")
            .RequireAuthorization()
            .HasApiVersion(1.0);

        group.MapGet("/", async (ISender sender) =>
        {
            var result = await sender.Send(new ListUserProjectQuery());

            return result.OperationResult switch
            {
                OperationResult.Failed => Results.Problem(),
                OperationResult.NotFound => Results.NotFound(),
                _ => Results.Ok(result.UserProjects),
            };
        })
        .Produces<IEnumerable<UserProjectDto>>()
        .Produces(404)
        .MapToApiVersion(1.0);

        group.MapGet("/{id:guid}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new GetUserProjectByIdQuery(id));

            return result.OperationResult switch
            {
                OperationResult.Failed => Results.Problem(),
                OperationResult.NotFound => Results.NotFound(),
                _ => Results.Ok(result.UserProject),
            };
        })
        .Produces<UserProjectDto>()
        .Produces(404)
        .MapToApiVersion(1.0);

        group.MapPost("/", async (CreateUserProjectCommand command, ISender sender) =>
        {
            var result = await sender.Send(command);

            return result switch
            {
                OperationResult.Failed => Results.Problem(),
                OperationResult.NotFound => Results.NotFound(),
                _ => Results.Created(),
            };
        })
        .Accepts<CreateUserProjectCommand>("application/json")
        //.Produces<UserProjectDto>(201)
        .Produces(201)
        .Produces(400)
        .MapToApiVersion(1.0);

        group.MapPatch("/{id:guid}", async (Guid id, UpdateUserProjectCommand command, ISender sender) =>
        {
            var result = await sender.Send(command);

            return result switch
            {
                OperationResult.Failed => Results.Problem(),
                OperationResult.NotFound => Results.NotFound(),
                _ => Results.NoContent(),
            };
        })
        .Accepts<UpdateUserProjectCommand>("application/json")
        .Produces(204)
        .Produces(400)
        .Produces(404)
        .MapToApiVersion(1.0);

        group.MapDelete("/{id:guid}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new RemoveUserProjectCommand(id));

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
