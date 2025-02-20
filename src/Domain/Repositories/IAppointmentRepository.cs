namespace Domain.Repositories;

public interface IAppointmentRepository
{
    Task<bool> CreateAppointment(Appointment appointment);
    Task<Appointment?> GetAppointmentById(Guid id);
    Task<List<Appointment?>?> ListAppointments();
    Task<bool> RemoveAppointment(Guid id);
    Task<bool> UpdateAppointment(Guid id, string description, DateTime keepDate, int amountHours, Guid assignmentId, Guid userId);
}
