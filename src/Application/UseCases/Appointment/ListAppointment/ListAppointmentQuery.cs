namespace Application.UseCases.Appointment.ListAppointment;

public sealed record ListAppointmentQuery(PaginationParams Pagination) : BaseQuery, IRequest<ListAppointmentQueryViewModel>;
