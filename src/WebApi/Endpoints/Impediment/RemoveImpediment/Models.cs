namespace WebApi.Endpoints.Impediment.RemoveImpediment;

/// <summary>
/// Request model for removing an impediment.
/// </summary>
public class Request : RemoveRequest
{
    public new class Validator : RemoveRequest.Validator
    {
    }
}

/// <summary>
/// Response model for the removal of an impediment.
/// </summary>
public class Response : RemoveResponse
{
}
