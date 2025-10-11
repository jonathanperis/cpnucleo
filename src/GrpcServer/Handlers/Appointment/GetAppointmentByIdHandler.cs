namespace GrpcServer.Handlers.Appointment;

public sealed class GetAppointmentByIdHandler : ICommandHandler<GetAppointmentByIdCommand, GetAppointmentByIdResult>
{
    public async Task<GetAppointmentByIdResult> ExecuteAsync(GetAppointmentByIdCommand command, CancellationToken cancellationToken)
    {
        return null;
    }
}