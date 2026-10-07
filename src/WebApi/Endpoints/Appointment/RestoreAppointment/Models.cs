namespace WebApi.Endpoints.Appointment.RestoreAppointment;

/// <summary>
/// Request model for restoring removed appointments.
/// </summary>
public class RestoreAppointmentRequest : RestoreRequest
{
    public class Validator : RestoreRequestValidator<RestoreAppointmentRequest>
    {
    }
}

/// <summary>
/// Response model for the restore of appointments.
/// </summary>
public class Response : RestoreResponse
{
}
