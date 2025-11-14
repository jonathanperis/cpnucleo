namespace WebApi.Endpoints.AssignmentImpediment.RemoveAssignmentImpediment;

/// <summary>
/// Request model for removing an assignment impediment.
/// </summary>
public class Request : RemoveRequest
{
    public new class Validator : RemoveRequest.Validator
    {
    }
}

/// <summary>
/// Response model for the removal of an assignment impediment.
/// </summary>
public class Response : RemoveResponse
{
}
