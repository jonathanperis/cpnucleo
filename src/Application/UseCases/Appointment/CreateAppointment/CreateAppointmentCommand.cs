namespace Application.UseCases.Appointment.CreateAppointment;

public sealed record CreateAppointmentCommand(string Description, DateTime KeepDate, int AmountHours, Guid AssignmentId, Guid UserId, Guid Id = default) : BaseCommand, IRequest<OperationResult>;
