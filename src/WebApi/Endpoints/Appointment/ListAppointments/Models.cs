namespace WebApi.Endpoints.Appointment.ListAppointments;

/// <summary>
/// Request model for listing appointments.
/// </summary>
public class Request
{
    /// <summary>
    /// Gets or sets the pagination parameters for the request.
    /// </summary>
    [FromQuery]
    public PaginationParams Pagination { get; set; } = new();

    public class Validator : Validator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.Pagination).ValidPagination(typeof(Domain.Entities.Appointment));
        }
    }    
}

/// <summary>
/// Response model for the list of appointments.
/// </summary>
public class Response
{
    /// <summary>
    /// Gets or sets the paginated result of appointments.
    /// </summary>
    public PaginatedResult<AppointmentDto?> Result { get; set; } = null!;
}
