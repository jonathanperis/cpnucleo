namespace WebApi.Endpoints.Appointment.CreateAppointment;

// EF Core
public class Endpoint(IApplicationDbContext dbContext) : Endpoint<Request, Response>
{
    public override void Configure()
    {
        Post("/api/appointment");
        Description(x => x.WithTags("Appointments"));
        AllowAnonymous();

        Summary(s =>
        {
            s.Summary = "Create a new appointment";
            s.Description = "Creates a new appointment record with the given data and custom Id. Validates uniqueness and returns the created appointment's data.";
        });
    }

    public override async Task HandleAsync(Request request, CancellationToken cancellationToken)
    {
        try
        {
            Logger.LogInformation("Service started processing request with payload Name: {Name}, Id: {AppointmentId}", request.Name, request.Id);

            Logger.LogInformation("Checking if an appointment entity exists with Id: {AppointmentId}", request.Id);
            var itemExists = dbContext.Appointments!.Any(x => x.Id == request.Id);

            if (itemExists)
            {
                Logger.LogWarning("Appointment Id conflict for Id: {AppointmentId}", request.Id);
                AddError(r => r.Id, "this Id is already in use!");
            }

            ThrowIfAnyErrors();

            Logger.LogInformation("Validation passed, proceeding to create new appointment entity.");
            var newItem = Domain.Entities.Appointment.Create(request.Description,
                                                             request.KeepDate,
                                                             request.AmountHours,
                                                             request.AssignmentId,
                                                             request.UserId,
                                                             request.Id);
            Logger.LogInformation("Created new appointment entity with Id: {AppointmentId}", newItem.Id);

            Logger.LogInformation("Adding appointment to repository.");
            await dbContext.Appointments!.AddAsync(newItem, cancellationToken);

            Logger.LogInformation("Committing transaction.");
            await dbContext.SaveChangesAsync(cancellationToken);

            Logger.LogInformation("Fetching appointment by Id: {AppointmentId}", newItem.Id);
            var createdItem = await dbContext.Appointments!.FindAsync(newItem.Id, cancellationToken);

            Response.Appointment = createdItem!.MapToDto();

            Logger.LogInformation("Service completed successfully.");

            await SendOkAsync(Response, cancellation: cancellationToken);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "An error occurred while processing the request.");
            ThrowError("An error has occurred.");
        }
    }
}
