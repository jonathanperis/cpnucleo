namespace WebApi.Endpoints.Appointment.RemoveAppointment;

/// <summary>
/// Request model for removing an appointment.
/// </summary>
public class Request : RemoveRequest
{
    public new class Validator : RemoveRequest.Validator
    {
    }
}

/// <summary>
/// Response model for the removal of an appointment.
/// </summary>
public class Response : RemoveResponse
{
}
