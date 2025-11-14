namespace WebApi.Endpoints.UserAssignment.RemoveUserAssignment;

/// <summary>
/// Request model for removing a user assignment.
/// </summary>
public class Request : RemoveRequest
{
    public new class Validator : RemoveRequest.Validator
    {
    }
}

/// <summary>
/// Response model for the removal of a user assignment.
/// </summary>
public class Response : RemoveResponse
{
}
