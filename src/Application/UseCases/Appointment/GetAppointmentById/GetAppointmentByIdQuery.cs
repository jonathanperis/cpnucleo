namespace Application.UseCases.Appointment.GetAppointmentById;

public sealed record GetAppointmentByIdQuery(Ulid Id) : IRequest<GetAppointmentByIdQueryViewModel>;
