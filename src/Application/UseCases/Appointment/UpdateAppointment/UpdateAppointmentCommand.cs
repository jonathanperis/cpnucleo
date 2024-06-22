namespace Application.UseCases.Appointment.UpdateAppointment;

public sealed record UpdateAppointmentCommand(Ulid Id, string Description, DateTime KeepDate, byte AmountHours, Ulid AssignmentId, Ulid UserId) : BaseCommand, IRequest<OperationResult>;
