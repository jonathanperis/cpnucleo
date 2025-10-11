namespace GrpcServer.Handlers.Appointment;

public sealed class ListAppointmentsHandler : ICommandHandler<ListAppointmentsCommand, ListAppointmentsResult>
{
    public async Task<ListAppointmentsResult> ExecuteAsync(ListAppointmentsCommand command, CancellationToken cancellationToken)
    {
        return null;
    }
}