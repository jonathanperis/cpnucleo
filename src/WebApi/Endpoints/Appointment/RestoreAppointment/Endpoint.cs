namespace WebApi.Endpoints.Appointment.RestoreAppointment;

// EF Core
public class Endpoint(IApplicationDbContext dbContext, ListingChangeNotifier listings) : Endpoint<RestoreAppointmentRequest, Response>
{
    public override void Configure()
    {
        Post("/appointment/restore");
        Description(x => x.WithTags("Appointments"));

        Summary(s =>
        {
            s.Summary = "Restore removed appointments by Ids";
            s.Description = "Undoes the soft delete of the appointments specified by the provided Ids in one transaction. If any id is missing, still active or not visible, nothing is restored.";
            s.Responses[404] = "An id is missing, active or not visible; nothing was restored.";
        });
    }

    public override async Task HandleAsync(RestoreAppointmentRequest request, CancellationToken cancellationToken)
    {
        var ids = BatchIds.Normalize(request.Ids, "restored");
        Logger.LogInformation("Checking that {Count} appointment entities are removed.", ids.Length);
        var items = await dbContext.Appointments!.IgnoreQueryFilters().Where(x => ids.Contains(x.Id) && !x.Active).ToListAsync(cancellationToken);

        if (items.Count != ids.Length)
        {
            Logger.LogWarning("At least one appointment to restore was not found or is not removed; nothing was restored.");
            await Send.NotFoundEnvelopeAsync(cancellationToken);
            return;
        }

        Logger.LogInformation("Restoring {Count} appointment entities in one transaction.", items.Count);
        foreach (var item in items) Domain.Entities.Appointment.Restore(item);

        // A single SaveChanges is one transaction: every restore succeeds or none does. Access checks
        // (AccessGuardInterceptor) and the relationship triggers apply as for any other write.
        Response.Success = await dbContext.SaveChangesAsync(cancellationToken);

        Logger.LogInformation("Restore result: {Success}", Response.Success);

        if (Response.Success) listings.NotifyChanged(nameof(Domain.Entities.Appointment));

        await Send.OkAsync(Response, cancellationToken);
    }
}
