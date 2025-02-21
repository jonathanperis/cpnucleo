namespace Application.UseCases.Appointment.CreateAppointment;

// EF Core
public sealed class CreateAppointmentCommandHandler(IApplicationDbContext dbContext) : IRequestHandler<CreateAppointmentCommand, OperationResult>
{
    public async ValueTask<OperationResult> Handle(CreateAppointmentCommand request, CancellationToken cancellationToken)
    {
        var appointment = Domain.Entities.Appointment.Create(request.Description, request.KeepDate, request.AmountHours, request.AssignmentId, request.UserId, request.Id);

        if (dbContext.Appointments is not null)
            await dbContext.Appointments.AddAsync(appointment, cancellationToken);

        var result = await dbContext.SaveChangesAsync(cancellationToken);

        return result ? OperationResult.Success : OperationResult.Failed;
    }
}
