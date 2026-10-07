namespace WebApi.Endpoints.Organization.RestoreOrganization;

/// <summary>
/// Request model for restoring removed organizations.
/// </summary>
public class RestoreOrganizationRequest : RestoreRequest
{
    public class Validator : RestoreRequestValidator<RestoreOrganizationRequest>
    {
    }
}

/// <summary>
/// Response model for the restore of organizations.
/// </summary>
public class Response : RestoreResponse
{
}
