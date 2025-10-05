namespace WebApi.Endpoints.Appointment.ListAppointments;

/// <summary>
/// Request model for listing appointments.
/// </summary>
public class Request
{
    /// <summary>
    /// Gets or sets the pagination parameters for the request.
    /// </summary>
    public required PaginationParams Pagination { get; set; }
}

/// <summary>
/// Response model for the list of appointments.
/// </summary>
public class Response
{
    /// <summary>
    /// Gets or sets the paginated result of appointments.
    /// </summary>
    public required PaginatedResult<AppointmentDto?> Result { get; set; }
}
