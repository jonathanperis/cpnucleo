namespace WebApi.Endpoints.Assignment.RemoveAssignment;

/// <summary>
/// Request model for removing an assignment.
/// </summary>
public class RemoveAssignmentRequest : RemoveRequest
{
    public class Validator : RemoveRequestValidator<RemoveAssignmentRequest>
    {
    }
}

/// <summary>
/// Response model for the removal of an assignment.
/// </summary>
public class Response : RemoveResponse
{
}
