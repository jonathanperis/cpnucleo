namespace WebApi.Endpoints.Assignment.RemoveAssignment;

/// <summary>
/// Request model for removing an assignment.
/// </summary>
public class Request : RemoveRequest
{
    public new class Validator : RemoveRequest.Validator
    {
    }
}

/// <summary>
/// Response model for the removal of an assignment.
/// </summary>
public class Response : RemoveResponse
{
}
