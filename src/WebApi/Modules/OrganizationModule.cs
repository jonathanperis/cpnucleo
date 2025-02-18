namespace WebApi.Modules;

public static class OrganizationModule
{
    public static void MapOrganizationEndpoints(this IVersionedEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/api/organizations")
            .WithTags("Organizations")
            .HasApiVersion(1.0)
            .RequireAuthorization();

        group.MapGet("/", async (ISender sender) =>
        {
            var result = await sender.Send(new ListOrganizationQuery());

            return result.OperationResult switch
            {
                OperationResult.Failed => Results.Problem(),
                OperationResult.NotFound => Results.NotFound(),
                _ => Results.Ok(result.Organizations),
            };
        })
        .Produces<IEnumerable<OrganizationDto>>()
        .Produces(404)
        .MapToApiVersion(1.0);

        group.MapGet("/{id:guid}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new GetOrganizationByIdQuery(id));

            return result.OperationResult switch
            {
                OperationResult.Failed => Results.Problem(),
                OperationResult.NotFound => Results.NotFound(),
                _ => Results.Ok(result.Organization),
            };
        })
        .Produces<OrganizationDto>()
        .Produces(404)
        .MapToApiVersion(1.0);

        group.MapPost("/", async (CreateOrganizationCommand command, ISender sender) =>
        {
            var result = await sender.Send(command);

            return result switch
            {
                OperationResult.Failed => Results.Problem(),
                OperationResult.NotFound => Results.NotFound(),
                _ => Results.Created(),
            };
        })
        .Accepts<CreateOrganizationCommand>("application/json")
        //.Produces<OrganizationDto>(201)
        .Produces(201)
        .Produces(400)
        .MapToApiVersion(1.0);

        group.MapPatch("/{id:guid}", async (Guid id, UpdateOrganizationCommand command, ISender sender) =>
        {
            var result = await sender.Send(command);

            return result switch
            {
                OperationResult.Failed => Results.Problem(),
                OperationResult.NotFound => Results.NotFound(),
                _ => Results.NoContent(),
            };
        })
        .Accepts<UpdateOrganizationCommand>("application/json")
        .Produces(204)
        .Produces(400)
        .Produces(404)
        .MapToApiVersion(1.0);

        group.MapDelete("/{id:guid}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new RemoveOrganizationCommand(id));

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
