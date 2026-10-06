namespace WebApi.Endpoints.UserAssignment.RemoveUserAssignment;

/// <summary>
/// Request model for removing a user assignment.
/// </summary>
public class RemoveUserAssignmentRequest : RemoveRequest
{
    public class Validator : RemoveRequestValidator<RemoveUserAssignmentRequest>
    {
    }
}

/// <summary>
/// Response model for the removal of a user assignment.
/// </summary>
public class Response : RemoveResponse
{
}
