namespace GrpcServer.Contracts.Commands.Appointment;

/// <summary>
/// Command model for fetching an appointment by its unique identifier.
/// </summary>
public class GetAppointmentByIdCommand : ICommand<GetAppointmentByIdResult>
{
    /// <summary>
    /// Gets or sets the unique identifier for the appointment.
    /// </summary>
    public Guid Id { get; set; }
}

/// <summary>
/// Result model for the appointment fetched by its unique identifier.
/// </summary>
public class GetAppointmentByIdResult
{
    /// <summary>
    /// Gets or sets a value indicating whether the fetch was successful.
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// Gets or sets a message providing additional information about the result.
    /// </summary>
    public string Message { get; set; }
    
    /// <summary>
    /// Gets or sets the appointment details.
    /// </summary>
    public AppointmentDto? Appointment { get; set; }
}
