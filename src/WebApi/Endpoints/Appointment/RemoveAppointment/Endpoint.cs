namespace WebApi.Endpoints.Appointment.RemoveAppointment;

// EF Core
public class Endpoint(IApplicationDbContext dbContext, ListingChangeNotifier listings) : Endpoint<RemoveAppointmentRequest, Response>
{
    public override void Configure()
    {
        Delete("/appointment");
        Description(x => x.WithTags("Appointments"));

        Summary(s =>
        {
            s.Summary = "Delete appointments by Ids";
            s.Description = "Deletes the appointments specified by the provided Ids. Validates existence of each, removes them, updates the repository, and commits the transaction.";
            s.Responses[404] = "An id is missing or not visible; nothing was removed.";
            s.Responses[409] = "A record still has active dependent data; nothing was removed.";
        });
    }

    public override async Task HandleAsync(RemoveAppointmentRequest request, CancellationToken cancellationToken)
    {
        Logger.LogInformation("Checking if appointment entities exist for Ids: {AppointmentIds}", string.Join(",", request.Ids));
        var ids = BatchIds.Normalize(request.Ids);
        var items = await dbContext.Appointments!.Where(x => ids.Contains(x.Id)).ToListAsync(cancellationToken);

        if (items.Count != ids.Length)
        {
            Logger.LogWarning("At least one appointment to remove was not found; nothing was removed.");
            await Send.NotFoundEnvelopeAsync(cancellationToken);
            return;
        }

        Logger.LogInformation("Removing {Count} appointment entities in one transaction.", items.Count);
        foreach (var item in items) Domain.Entities.Appointment.Remove(item);

        // A single SaveChanges is one transaction: every removal succeeds or none does.
        Response.Success = await dbContext.SaveChangesAsync(cancellationToken);

        Logger.LogInformation("Remove result: {Success}", Response.Success);

        if (Response.Success) listings.NotifyChanged(nameof(Domain.Entities.Appointment));

        await Send.OkAsync(Response, cancellationToken);
    }
}
