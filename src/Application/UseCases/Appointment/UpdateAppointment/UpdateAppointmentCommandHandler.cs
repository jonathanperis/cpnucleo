namespace Application.UseCases.Appointment.UpdateAppointment;

public sealed class UpdateAppointmentCommandHandler(IApplicationDbContext dbContext) : IRequestHandler<UpdateAppointmentCommand, OperationResult>
{
    public async ValueTask<OperationResult> Handle(UpdateAppointmentCommand request, CancellationToken cancellationToken)
    {
        if (dbContext.Appointments is not null)
        {
            var appointment = await dbContext.Appointments
                .FirstOrDefaultAsync(p => p.Id == request.Id && p.Active, cancellationToken);

            if (appointment is null)
            {
                return OperationResult.NotFound;
            }

            Domain.Entities.Appointment.Update(appointment, request.Description, request.KeepDate, request.AmountHours, request.AssignmentId, request.UserId);
        }

        var result = await dbContext.SaveChangesAsync(cancellationToken);

        return result ? OperationResult.Success : OperationResult.Failed;
    }
}
