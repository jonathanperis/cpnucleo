namespace WebApi.Endpoints.Organization.ListOrganizations;

/// <summary>
/// Request model for listing organizations.
/// </summary>
public class Request
{
    /// <summary>
    /// Gets or sets the pagination parameters for the request.
    /// </summary>
    public PaginationParams? Pagination { get; set; }
}

/// <summary>
/// Response model for the list of organizations.
/// </summary>
public class Response
{   
    /// <summary>
    /// Gets or sets the paginated result of organizations.
    /// </summary>
    public PaginatedResult<OrganizationDto>? Result { get; set; }
}