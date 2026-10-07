namespace WebApi.Endpoints.Impediment.RestoreImpediment;

/// <summary>
/// Request model for restoring removed impediments.
/// </summary>
public class RestoreImpedimentRequest : RestoreRequest
{
    public class Validator : RestoreRequestValidator<RestoreImpedimentRequest>
    {
    }
}

/// <summary>
/// Response model for the restore of impediments.
/// </summary>
public class Response : RestoreResponse
{
}
