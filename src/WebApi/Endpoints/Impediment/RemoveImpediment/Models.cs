namespace WebApi.Endpoints.Impediment.RemoveImpediment;

/// <summary>
/// Request model for removing an impediment.
/// </summary>
public class RemoveImpedimentRequest : RemoveRequest
{
    public class Validator : RemoveRequestValidator<RemoveImpedimentRequest>
    {
    }
}

/// <summary>
/// Response model for the removal of an impediment.
/// </summary>
public class Response : RemoveResponse
{
}
