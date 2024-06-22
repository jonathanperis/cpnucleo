namespace Application.Common.Repositories;

public interface IAppointmentRepository
{
    Task<bool> CreateAppointment(Appointment appointment);
    Task<AppointmentDto?> GetAppointmentById(Ulid id);
    Task<List<AppointmentDto>?> ListAppointments();
    Task<bool> RemoveAppointment(Ulid id);
    Task<bool> UpdateAppointment(Ulid id, string description, DateTime keepDate, byte amountHours, Ulid assignmentId, Ulid userId);
}
