namespace WebApi.Endpoints.Appointment.UpdateAppointment;

// EF Core
public class Endpoint(IApplicationDbContext dbContext) : Endpoint<Request, Response>
{
    public override void Configure()
    {
        Patch("/api/appointment");
        Description(x => x.WithTags("Appointments"));
        AllowAnonymous();

        Summary(s =>
        {
            s.Summary = "Update an existing appointment";
            s.Description = "Updates the appointment identified by the provided Id with new given data. Validates existence and returns whether the update was successful.";
        });
    }

    public override async Task HandleAsync(Request request, CancellationToken cancellationToken)
    {
        Logger.LogInformation("Service started processing request.");

        try
        {
            Logger.LogInformation("Checking if an appointment entity exists with Id: {AppointmentId}", request.Id);
            var item = await dbContext.Appointments!.FindAsync(request.Id, cancellationToken);

            if (item is null)
            {
                await SendNotFoundAsync(cancellation: cancellationToken);
                return;
            }

            Logger.LogInformation("Updating appointment entity with Id: {AppointmentId}", request.Id);
            Domain.Entities.Appointment.Update(item,
                                               request.Description,
                                               request.KeepDate,
                                               request.AmountHours,
                                               request.AssignmentId,
                                               request.UserId);

            Logger.LogInformation("Updating entity in repository.");
            Response.Success = await dbContext.SaveChangesAsync(cancellationToken);

            Logger.LogInformation("Update result: {Success}", Response.Success);
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
