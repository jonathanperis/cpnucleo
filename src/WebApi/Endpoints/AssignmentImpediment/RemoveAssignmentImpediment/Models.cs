namespace WebApi.Endpoints.AssignmentImpediment.RemoveAssignmentImpediment;

/// <summary>
/// Request model for removing an assignment impediment.
/// </summary>
public class RemoveAssignmentImpedimentRequest : RemoveRequest
{
    public class Validator : RemoveRequestValidator<RemoveAssignmentImpedimentRequest>
    {
    }
}

/// <summary>
/// Response model for the removal of an assignment impediment.
/// </summary>
public class Response : RemoveResponse
{
}
