namespace WebApi.Endpoints.AssignmentType.RemoveAssignmentType;

/// <summary>
/// Request model for removing an assignment type.
/// </summary>
public class RemoveAssignmentTypeRequest : RemoveRequest
{
    public class Validator : RemoveRequestValidator<RemoveAssignmentTypeRequest>
    {
    }
}

/// <summary>
/// Response model for the removal of an assignment type.
/// </summary>
public class Response : RemoveResponse
{
}
