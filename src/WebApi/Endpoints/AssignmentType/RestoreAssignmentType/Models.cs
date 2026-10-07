namespace WebApi.Endpoints.AssignmentType.RestoreAssignmentType;

/// <summary>
/// Request model for restoring removed assignment types.
/// </summary>
public class RestoreAssignmentTypeRequest : RestoreRequest
{
    public class Validator : RestoreRequestValidator<RestoreAssignmentTypeRequest>
    {
    }
}

/// <summary>
/// Response model for the restore of assignment types.
/// </summary>
public class Response : RestoreResponse
{
}
