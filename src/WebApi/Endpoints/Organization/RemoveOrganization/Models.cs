namespace WebApi.Endpoints.Organization.RemoveOrganization;

/// <summary>
/// Request model for removing an organization.
/// </summary>
public class Request : RemoveRequest
{
    public new class Validator : RemoveRequest.Validator
    {
    }
}

/// <summary>
/// Response model for the removal of an organization.
/// </summary>
public class Response : RemoveResponse
{
}