namespace WebApi.Modules;

public static class AppointmentModule
{
    public static void MapAppointmentEndpoints(this IVersionedEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/api/appointments")
            .WithTags("Appointments")
            .RequireAuthorization()
            .HasApiVersion(1.0);

        group.MapGet("/", async (ISender sender) =>
        {
            var result = await sender.Send(new ListAppointmentQuery());

            return result.OperationResult switch
            {
                OperationResult.Failed => Results.Problem(),
                OperationResult.NotFound => Results.NotFound(),
                _ => Results.Ok(result.Appointments),
            };
        })
        .Produces<IEnumerable<AppointmentDto>>()
        .Produces(404)
        .MapToApiVersion(1.0);

        group.MapGet("/{id:guid}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new GetAppointmentByIdQuery(id));

            return result.OperationResult switch
            {
                OperationResult.Failed => Results.Problem(),
                OperationResult.NotFound => Results.NotFound(),
                _ => Results.Ok(result.Appointment),
            };
        })
        .Produces<AppointmentDto>()
        .Produces(404)
        .MapToApiVersion(1.0);

        group.MapPost("/", async (CreateAppointmentCommand command, ISender sender) =>
        {
            var result = await sender.Send(command);

            return result switch
            {
                OperationResult.Failed => Results.Problem(),
                OperationResult.NotFound => Results.NotFound(),
                _ => Results.Created(),
            };
        })
        .Accepts<CreateAppointmentCommand>("application/json")
        //.Produces<AppointmentDto>(201)
        .Produces(201)
        .Produces(400)
        .MapToApiVersion(1.0);

        group.MapPatch("/{id:guid}", async (Guid id, UpdateAppointmentCommand command, ISender sender) =>
        {
            var result = await sender.Send(command);

            return result switch
            {
                OperationResult.Failed => Results.Problem(),
                OperationResult.NotFound => Results.NotFound(),
                _ => Results.NoContent(),
            };
        })
        .Accepts<UpdateAppointmentCommand>("application/json")
        .Produces(204)
        .Produces(400)
        .Produces(404)
        .MapToApiVersion(1.0);

        group.MapDelete("/{id:guid}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new RemoveAppointmentCommand(id));

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
