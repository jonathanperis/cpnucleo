namespace Application.UseCases.Appointment.CreateAppointment;

public sealed record CreateAppointmentCommand(string Description, DateTime KeepDate, byte AmountHours, Guid AssignmentId, Guid UserId, Guid Id = default) : BaseCommand, IRequest<OperationResult>;
