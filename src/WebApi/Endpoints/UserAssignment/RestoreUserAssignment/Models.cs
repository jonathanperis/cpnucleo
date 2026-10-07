namespace WebApi.Endpoints.UserAssignment.RestoreUserAssignment;

/// <summary>
/// Request model for restoring removed user assignments.
/// </summary>
public class RestoreUserAssignmentRequest : RestoreRequest
{
    public class Validator : RestoreRequestValidator<RestoreUserAssignmentRequest>
    {
    }
}

/// <summary>
/// Response model for the restore of user assignments.
/// </summary>
public class Response : RestoreResponse
{
}
