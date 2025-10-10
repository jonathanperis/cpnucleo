namespace GrpcServer.Contracts.Commands.Organization;

/// <summary>
/// Command model for fetching an organization by its unique identifier.
/// </summary>
public class GetOrganizationByIdCommand
{
    /// <summary>
    /// Gets or sets the unique identifier for the organization.
    /// </summary>
    public Guid Id { get; set; }
}

/// <summary>
/// Result model for the organization fetched by its unique identifier.
/// </summary>
public class GetOrganizationByIdResult
{   
    /// <summary>
    /// Gets or sets the organization details.
    /// </summary>
    public OrganizationDto? Organization { get; set; }
}