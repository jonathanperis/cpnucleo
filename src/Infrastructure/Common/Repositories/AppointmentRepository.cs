namespace Infrastructure.Common.Repositories;

//[DapperAot]
public class AppointmentRepository(IConfiguration configuration) : IAppointmentRepository
{
    public async Task<bool> CreateAppointment(Appointment appointment)
    {
        await using var connection = new NpgsqlConnection(configuration.GetConnectionString("DefaultConnection"));

        const string sql = """
                           INSERT INTO "Appointment" ("Id", "Description", "KeepDate", "AmountHours", "AssignmentId", "UserId", "CreatedAt", "Active")
                           VALUES (@Id, @Description, @KeepDate, @AmountHours, @AssignmentId, @UserId, @CreatedAt, @Active);
                           """;

        return await connection.ExecuteAsync(sql, new { appointment.Id, appointment.Description, appointment.KeepDate, appointment.AmountHours, appointment.AssignmentId, appointment.UserId, appointment.CreatedAt, appointment.Active }) == 1;
    }

    public async Task<AppointmentDto?> GetAppointmentById(Ulid id)
    {
        await using var connection = new NpgsqlConnection(configuration.GetConnectionString("DefaultConnection"));

        const string sql = """
                           SELECT "Id", "Description", "KeepDate", "AmountHours", "AssignmentId", "UserId", "CreatedAt", "UpdatedAt", "Active"
                           FROM "Appointment"
                           WHERE "Id" = @Id AND "Active" = 1;
                           """;

        return await connection.QueryFirstOrDefaultAsync<AppointmentDto>(sql, new { Id = id });
    }

    public async Task<List<AppointmentDto>?> ListAppointments()
    {
        await using var connection = new NpgsqlConnection(configuration.GetConnectionString("DefaultConnection"));

        const string sql = """
                           SELECT "Id", "Description", "KeepDate", "AmountHours", "AssignmentId", "UserId", "CreatedAt", "UpdatedAt", "Active"
                           FROM "Appointment"
                           WHERE "Active" = 1;
                           """;

        return (await connection.QueryAsync<AppointmentDto>(sql)).AsList();
    }

    public async Task<bool> RemoveAppointment(Ulid id)
    {
        await using var connection = new NpgsqlConnection(configuration.GetConnectionString("DefaultConnection"));

        const string sql = """
                           UPDATE "Appointment"
                           SET "Active" = 0, "DeletedAt" = @DeletedAt
                           WHERE "Id" = @Id;
                           """;

        var deletedAt = DateTime.UtcNow;

        return await connection.ExecuteAsync(sql, new { DeletedAt = deletedAt, Id = id }) == 1;
    }

    public async Task<bool> UpdateAppointment(Ulid id, string description, DateTime keepDate, byte amountHours, Ulid assignmentId, Ulid userId)
    {
        await using var connection = new NpgsqlConnection(configuration.GetConnectionString("DefaultConnection"));

        const string sql = """
                           UPDATE "Appointment"
                           SET "Description" = @Description, "KeepDate" = @KeepDate, "AmountHours" = @AmountHours, "AssignmentId" = @AssignmentId, "UserId" = @UserId, "UpdatedAt" = @UpdatedAt
                           WHERE "Id" = @Id;
                           """;

        var updatedAt = DateTime.UtcNow;

        return await connection.ExecuteAsync(sql, new { Description = description, KeepDate = keepDate, AmountHours = amountHours, AssignmentId = assignmentId, UserId = userId, UpdatedAt = updatedAt, Id = id }) == 1;
    }
}
