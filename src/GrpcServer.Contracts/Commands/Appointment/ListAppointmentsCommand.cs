namespace GrpcServer.Contracts.Commands.Appointment;

/// <summary>
/// Command model for listing appointments.
/// </summary>
public class ListAppointmentsCommand : ICommand<ListAppointmentsResult>
{
    /// <summary>
    /// Gets or sets the pagination parameters for the request.
    /// </summary>
    public required PaginationParams Pagination { get; set; } 
}

/// <summary>
/// Result model for the list of appointments.
/// </summary>
public class ListAppointmentsResult
{
    /// <summary>
    /// Gets or sets the paginated result of appointments.
    /// </summary>
    public required PaginatedResult<AppointmentDto?> Result { get; set; }
}
