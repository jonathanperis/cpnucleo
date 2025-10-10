namespace GrpcServer.Contracts.Commands.Appointment;

/// <summary>
/// Command model for removing an appointment.
/// </summary>
public class RemoveAppointmentCommand
{
    /// <summary>
    /// Gets or sets the unique identifiers for the appointments.
    /// </summary>
    public required List<Guid> Ids { get; set; }
}

/// <summary>
/// Result model for the removal of an appointment.
/// </summary>
public class RemoveAppointmentResult
{
    /// <summary>
    /// Gets or sets a value indicating whether the removal was successful.
    /// </summary>
    public bool Success { get; set; }
}
