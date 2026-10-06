namespace WebApi.Endpoints.Appointment.RemoveAppointment;

/// <summary>
/// Request model for removing an appointment.
/// </summary>
public class RemoveAppointmentRequest : RemoveRequest
{
    public class Validator : RemoveRequestValidator<RemoveAppointmentRequest>
    {
    }
}

/// <summary>
/// Response model for the removal of an appointment.
/// </summary>
public class Response : RemoveResponse
{
}
