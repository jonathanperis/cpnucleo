namespace WebApi.Endpoints.Organization.RemoveOrganization;

/// <summary>
/// Request model for removing an organization.
/// </summary>
public class RemoveOrganizationRequest : RemoveRequest
{
    public class Validator : RemoveRequestValidator<RemoveOrganizationRequest>
    {
    }
}

/// <summary>
/// Response model for the removal of an organization.
/// </summary>
public class Response : RemoveResponse
{
}
