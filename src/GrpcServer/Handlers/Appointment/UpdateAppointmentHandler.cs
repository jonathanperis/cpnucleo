namespace GrpcServer.Handlers.Appointment;

public sealed class UpdateAppointmentHandler : ICommandHandler<UpdateAppointmentCommand, UpdateAppointmentResult>
{
    public async Task<UpdateAppointmentResult> ExecuteAsync(UpdateAppointmentCommand command, CancellationToken cancellationToken)
    {
        return null;
    }
}