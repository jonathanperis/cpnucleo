namespace WebApi.Endpoints.Appointment.GetAppointmentById;

// Dapper Repository Advanced
public class Endpoint(IUnitOfWork unitOfWork) : Endpoint<Request, Response>
{
    public override void Configure()
    {
        Get("/appointment");
        Description(x => x.WithTags("Appointments"));

        Summary(s =>
        {
            s.Summary = "Retrieve an appointment by Id";
            s.Description = "Fetches the appointment matching the provided Id. Returns 404 if not found, otherwise returns the appointment data mapped to a DTO.";
        });
    }

    public override async Task HandleAsync(Request request, CancellationToken cancellationToken)
    {
        Logger.LogInformation("Fetching appointment entity with Id: {AppointmentId}", request.Id);
        var repository = unitOfWork.GetRepository<Domain.Entities.Appointment>();
        var item = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (item is null)
        {
            Logger.LogWarning("Appointment not found with Id: {AppointmentId}", request.Id);
            await Send.NotFoundEnvelopeAsync(cancellationToken);
            return;
        }

        Logger.LogInformation("Mapping entity to DTO and setting response for Id: {AppointmentId}", request.Id);
        Response.Appointment = item.MapToDto();

        await Send.OkAsync(Response, cancellationToken);
    }
}
