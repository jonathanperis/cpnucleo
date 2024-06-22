namespace Application.UseCases.Appointment.CreateAppointment;

public sealed record CreateAppointmentCommand(string Description, DateTime KeepDate, byte AmountHours, Ulid AssignmentId, Ulid UserId, Ulid Id = default) : BaseCommand, IRequest<OperationResult>;
