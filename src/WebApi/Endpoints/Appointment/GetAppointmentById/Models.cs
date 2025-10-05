namespace WebApi.Endpoints.Appointment.GetAppointmentById;

/// <summary>
/// Request model for fetching an appointment by its unique identifier.
/// </summary>
public class Request
{
    /// <summary>
    /// Gets or sets the unique identifier for the appointment.
    /// </summary>
    [DefaultValue("873eea4b-55c9-46a6-9512-4d59a77ad28b")]
    public Guid Id { get; set; }

    public class Validator : Validator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Id is required.");
        }
    }
}

/// <summary>
/// Response model for the appointment fetched by its unique identifier.
/// </summary>
public class Response
{
    /// <summary>
    /// Gets or sets the appointment details.
    /// </summary>
    public AppointmentDto? Appointment { get; set; }
}
