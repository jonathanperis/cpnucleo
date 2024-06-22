namespace Application.UseCases.Appointment.RemoveAppointment;

public sealed record RemoveAppointmentCommand(Ulid Id) : BaseCommand, IRequest<OperationResult>;
