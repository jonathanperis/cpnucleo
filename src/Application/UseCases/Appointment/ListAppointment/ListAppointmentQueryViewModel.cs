namespace Application.UseCases.Appointment.ListAppointment;

public sealed record ListAppointmentQueryViewModel(OperationResult OperationResult, PaginatedResult<AppointmentDto?> Result);
