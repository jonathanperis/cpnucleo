namespace Application.UseCases.Appointment.RemoveAppointment;

public sealed class RemoveAppointmentCommandHandler(IApplicationDbContext dbContext) : IRequestHandler<RemoveAppointmentCommand, OperationResult>
{
    public async ValueTask<OperationResult> Handle(RemoveAppointmentCommand request, CancellationToken cancellationToken)
    {
        if (dbContext.Appointments is not null)
        {
            var appointment = await dbContext.Appointments
                .FirstOrDefaultAsync(p => p.Id == request.Id && p.Active, cancellationToken);

            if (appointment == null)
            {
                return OperationResult.NotFound;
            }

            appointment = Domain.Entities.Appointment.Remove(appointment);
        }

        var result = await dbContext.SaveChangesAsync(cancellationToken);

        return result ? OperationResult.Success : OperationResult.Failed;
    }
}
