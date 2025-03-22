namespace Application.UseCases.Appointment.UpdateAppointment;

public sealed record UpdateAppointmentCommand(Guid Id, string Description, DateTime KeepDate, int AmountHours, Guid AssignmentId, Guid UserId) : BaseCommand, IRequest<OperationResult>;
