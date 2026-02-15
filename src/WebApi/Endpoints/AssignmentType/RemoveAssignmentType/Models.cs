namespace WebApi.Endpoints.AssignmentType.RemoveAssignmentType;

/// <summary>
/// Request model for removing an assignment type.
/// </summary>
public class Request : RemoveRequest
{
    public new class Validator : RemoveRequest.Validator
    {
    }
}

/// <summary>
/// Response model for the removal of an assignment type.
/// </summary>
public class Response : RemoveResponse
{
}
