namespace Application.UseCases.Appointment.GetAppointmentById;

public sealed record GetAppointmentByIdQuery(Guid Id) : IRequest<GetAppointmentByIdQueryViewModel>;
