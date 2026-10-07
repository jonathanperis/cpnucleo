namespace WebApi.Endpoints.Assignment.RestoreAssignment;

/// <summary>
/// Request model for restoring removed assignments.
/// </summary>
public class RestoreAssignmentRequest : RestoreRequest
{
    public class Validator : RestoreRequestValidator<RestoreAssignmentRequest>
    {
    }
}

/// <summary>
/// Response model for the restore of assignments.
/// </summary>
public class Response : RestoreResponse
{
}
