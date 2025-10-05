namespace WebApi.Endpoints.Appointment.GetAppointmentById;

// Dapper Repository Advanced
public class Endpoint(IUnitOfWork unitOfWork) : Endpoint<Request, Response>
{
    public override void Configure()
    {
        Get("/api/appointment");
        Description(x => x.WithTags("Appointments"));
        AllowAnonymous();

        Summary(s =>
        {
            s.Summary = "Retrieve an appointment by Id";
            s.Description = "Fetches the appointment matching the provided Id. Returns 404 if not found, otherwise returns the appointment data mapped to a DTO.";
        });
    }

    public override async Task HandleAsync(Request request, CancellationToken cancellationToken)
    {
        Logger.LogInformation("Service started processing request.");

        Logger.LogInformation("Fetching appointment entity with Id: {AppointmentId}", request.Id);
        var repository = unitOfWork.GetRepository<Domain.Entities.Appointment>();
        var item = await repository.GetByIdAsync(request.Id);

        if (item is null)
        {
            Logger.LogWarning("Appointment not found with Id: {AppointmentId}", request.Id);
            await SendNotFoundAsync(cancellation: cancellationToken);
            return;
        }

        Logger.LogInformation("Mapping entity to DTO and setting response for Id: {AppointmentId}", request.Id);
        Response.Appointment = item!.MapToDto();

        Logger.LogInformation("Service completed successfully.");

        await SendOkAsync(Response, cancellation: cancellationToken);
    }
}
