namespace GrpcServer.Contracts.Commands.Appointment;

/// <summary>
/// Command model for creating a new appointment.
/// </summary>
public class CreateAppointmentCommand
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
/// Result model for the created appointment.
/// </summary>
public class CreateAppointmentResult
{
    /// <summary>
    /// Gets or sets the created appointment.
    /// </summary>
    public AppointmentDto? Appointment { get; set; }
}
