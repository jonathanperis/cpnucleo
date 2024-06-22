namespace Application.UseCases.Appointment.GetAppointmentById;

public sealed class GetAppointmentByIdQueryHandler(IAppointmentRepository appointmentRepository) : IRequestHandler<GetAppointmentByIdQuery, GetAppointmentByIdQueryViewModel>
{
    public async ValueTask<GetAppointmentByIdQueryViewModel> Handle(GetAppointmentByIdQuery request, CancellationToken cancellationToken)
    {
        var appointment = await appointmentRepository.GetAppointmentById(request.Id);

        var operationResult = appointment is not null ? OperationResult.Success : OperationResult.NotFound;

        return new GetAppointmentByIdQueryViewModel(operationResult, appointment);
    }
}
