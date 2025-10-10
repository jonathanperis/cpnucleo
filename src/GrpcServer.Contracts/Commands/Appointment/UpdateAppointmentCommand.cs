namespace GrpcServer.Contracts.Commands.Appointment;

/// <summary>
/// Command model for updating an appointment.
/// </summary>
public class UpdateAppointmentCommand
{
    /// <summary>
    /// Gets or sets the unique identifier for the appointment.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the appointment.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Gets or sets the description of the appointment.
    /// </summary>
    public required string Description { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the appointment should be kept.
    /// </summary>
    public DateTime KeepDate { get; set; }

    /// <summary>
    /// Gets or sets the amount of hours for the appointment.
    /// </summary>
    public int AmountHours { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier for the assignment associated with the appointment.
    /// </summary>
    public Guid AssignmentId { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier for the user associated with the appointment.
    /// </summary>
    public Guid UserId { get; set; }
}

/// <summary>
/// Result model for the updated appointment.
/// </summary>
public class UpdateAppointmentResult
{
    /// <summary>
    /// Gets or sets a value indicating whether the update was successful.
    /// </summary>
    public bool Success { get; set; }
}
