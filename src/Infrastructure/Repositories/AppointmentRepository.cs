namespace Infrastructure.Repositories;

//[DapperAot]
public class AppointmentRepository(NpgsqlConnection connection) : IAppointmentRepository
{
    public async Task<bool> CreateAppointment(Appointment appointment)
    {
        const string sql = """
                           INSERT INTO "Appointments" ("Id", "Description", "KeepDate", "AmountHours", "AssignmentId", "UserId", "CreatedAt", "Active")
                           VALUES (@Id, @Description, @KeepDate, @AmountHours, @AssignmentId, @UserId, @CreatedAt, @Active);
                           """;

        return await connection.ExecuteAsync(sql, new { appointment.Id, appointment.Description, appointment.KeepDate, appointment.AmountHours, appointment.AssignmentId, appointment.UserId, appointment.CreatedAt, appointment.Active }) == 1;
    }

    public async Task<Appointment?> GetAppointmentById(Guid id)
    {
        const string sql = """
                           SELECT "Id", "Description", "KeepDate", "AmountHours", "AssignmentId", "UserId", "CreatedAt", "UpdatedAt", "Active"
                           FROM "Appointments"
                           WHERE "Id" = @Id AND "Active" = true;
                           """;

        return await connection.QueryFirstOrDefaultAsync<Appointment>(sql, new { Id = id });
    }

    public async Task<List<Appointment?>?> ListAppointments()
    {
        const string sql = """
                           SELECT "Id", "Description", "KeepDate", "AmountHours", "AssignmentId", "UserId", "CreatedAt", "UpdatedAt", "Active"
                           FROM "Appointments"
                           WHERE "Active" = true;
                           """;

        return (await connection.QueryAsync<Appointment?>(sql)).AsList();
    }

    public async Task<bool> RemoveAppointment(Guid id)
    {
        const string sql = """
                           UPDATE "Appointments"
                           SET "Active" = 0, "DeletedAt" = @DeletedAt
                           WHERE "Id" = @Id;
                           """;

        var deletedAt = DateTime.UtcNow;

        return await connection.ExecuteAsync(sql, new { DeletedAt = deletedAt, Id = id }) == 1;
    }

    public async Task<bool> UpdateAppointment(Guid id, string description, DateTime keepDate, byte amountHours, Guid assignmentId, Guid userId)
    {
        const string sql = """
                           UPDATE "Appointments"
                           SET "Description" = @Description, "KeepDate" = @KeepDate, "AmountHours" = @AmountHours, "AssignmentId" = @AssignmentId, "UserId" = @UserId, "UpdatedAt" = @UpdatedAt
                           WHERE "Id" = @Id;
                           """;

        var updatedAt = DateTime.UtcNow;

        return await connection.ExecuteAsync(sql, new { Description = description, KeepDate = keepDate, AmountHours = amountHours, AssignmentId = assignmentId, UserId = userId, UpdatedAt = updatedAt, Id = id }) == 1;
    }
}
