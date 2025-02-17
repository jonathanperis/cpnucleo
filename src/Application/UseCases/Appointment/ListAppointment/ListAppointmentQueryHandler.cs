namespace Application.UseCases.Appointment.ListAppointment;

public sealed class ListAppointmentQueryHandler(IAppointmentRepository appointmentRepository) : IRequestHandler<ListAppointmentQuery, ListAppointmentQueryViewModel>
{
    public async ValueTask<ListAppointmentQueryViewModel> Handle(ListAppointmentQuery request, CancellationToken cancellationToken)
    {
        var appointments = await appointmentRepository.ListAppointments();

        var operationResult = appointments is not null ? OperationResult.Success : OperationResult.NotFound;

        var result = appointments?
                                            .Select(x => x?.MapToDto())
                                            .ToList();
        
        return new ListAppointmentQueryViewModel(operationResult, result);
    }
}
