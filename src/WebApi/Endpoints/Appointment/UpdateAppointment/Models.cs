namespace WebApi.Endpoints.Appointment.UpdateAppointment;

/// <summary>
/// Request model for updating an appointment.
/// </summary>
public class Request
{
    /// <summary>
    /// Gets or sets the unique identifier for the appointment.
    /// </summary>
    [DefaultValue("80aee820-ce09-4023-b62c-3a42ff4535b0")]
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the appointment.
    /// </summary>
    [DefaultValue("Updated Appointment")]
    public required string Name { get; set; }

    /// <summary>
    /// Gets or sets the description of the appointment.
    /// </summary>
    [DefaultValue("Updated Appointment description goes here")]
    public required string Description { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the appointment should be kept.
    /// </summary>
    [DefaultValue("2069-04-20")]
    public DateTime KeepDate { get; set; }

    /// <summary>
    /// Gets or sets the amount of hours for the appointment.
    /// </summary>
    [DefaultValue(4)]
    public int AmountHours { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier for the assignment associated with the appointment.
    /// </summary>
    [DefaultValue("35f1a233-e070-4205-909d-0eaabf89aec4")]
    public Guid AssignmentId { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier for the user associated with the appointment.
    /// </summary>
    [DefaultValue("35b9c5c1-6abf-4d50-aee8-00abe2f09560")]
    public Guid UserId { get; set; }

    public class Validator : Validator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Id is required.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required.");

            RuleFor(x => x.AmountHours)
                .NotEmpty().WithMessage("Amount of hours is required.");

            RuleFor(x => x.AmountHours)
                .GreaterThan(0).WithMessage("Amount of hours must be greater than 0.");

            RuleFor(x => x.AssignmentId)
                .NotEmpty().WithMessage("Assignment ID is required.");

            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("User ID is required.");
        }
    }
}

/// <summary>
/// Response model for the updated appointment.
/// </summary>
public class Response
{
    /// <summary>
    /// Gets or sets a value indicating whether the update was successful.
    /// </summary>
    public bool Success { get; set; }
}
