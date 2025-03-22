namespace Application.UseCases.Appointment.RemoveAppointment;

// EF Core
public sealed class RemoveAppointmentCommandHandler(IApplicationDbContext dbContext) : IRequestHandler<RemoveAppointmentCommand, OperationResult>
{
    public async ValueTask<OperationResult> Handle(RemoveAppointmentCommand request, CancellationToken cancellationToken)
    {
        if (dbContext.Appointments is not null)
        {
            var appointment = await dbContext.Appointments
                .FirstOrDefaultAsync(p => p.Id == request.Id && p.Active, cancellationToken);

            if (appointment is null)
            {
                return OperationResult.NotFound;
            }

            Domain.Entities.Appointment.Remove(appointment);
        }

        var result = await dbContext.SaveChangesAsync(cancellationToken);

        return result ? OperationResult.Success : OperationResult.Failed;
    }
}
