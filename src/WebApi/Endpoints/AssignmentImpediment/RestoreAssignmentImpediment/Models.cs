namespace WebApi.Endpoints.AssignmentImpediment.RestoreAssignmentImpediment;

/// <summary>
/// Request model for restoring removed assignment impediments.
/// </summary>
public class RestoreAssignmentImpedimentRequest : RestoreRequest
{
    public class Validator : RestoreRequestValidator<RestoreAssignmentImpedimentRequest>
    {
    }
}

/// <summary>
/// Response model for the restore of assignment impediments.
/// </summary>
public class Response : RestoreResponse
{
}
