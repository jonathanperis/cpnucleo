namespace Application.UseCases.Appointment.GetAppointmentById;

public sealed record GetAppointmentByIdQueryViewModel(OperationResult OperationResult, AppointmentDto? Appointment);
