namespace Application.UseCases.Appointment.RemoveAppointment;

public sealed record RemoveAppointmentCommand(Guid Id) : BaseCommand, IRequest<OperationResult>;
