namespace Application.UseCases.Appointment.ListAppointment;

public sealed class ListAppointmentQueryHandler(IAppointmentRepository appointmentRepository) : IRequestHandler<ListAppointmentQuery, ListAppointmentQueryViewModel>
{
    public async ValueTask<ListAppointmentQueryViewModel> Handle(ListAppointmentQuery request, CancellationToken cancellationToken)
    {
        var appointments = await appointmentRepository.ListAppointments();

        var operationResult = appointments is not null ? OperationResult.Success : OperationResult.NotFound;
        var appointmentsList = appointments ?? []; // Return an empty list if no appointments are found

        var result = appointmentsList.Select(appointment => (AppointmentDto)appointment).ToList();

        return new ListAppointmentQueryViewModel(operationResult, result);
    }
}
