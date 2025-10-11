namespace GrpcServer.Handlers.Appointment;

public sealed class RemoveAppointmentHandler : ICommandHandler<RemoveAppointmentCommand, RemoveAppointmentResult>
{
    public async Task<RemoveAppointmentResult> ExecuteAsync(RemoveAppointmentCommand command, CancellationToken cancellationToken)
    {
        return null;
    }
}