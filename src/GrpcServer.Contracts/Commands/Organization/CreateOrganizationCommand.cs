namespace GrpcServer.Contracts.Commands.Organization;

/// <summary>
/// Command model for creating a new organization.
/// </summary>
public class CreateOrganizationCommand
{
    /// <summary>
    /// Gets or sets the unique identifier for the organization.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the organization.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Gets or sets the description of the organization.
    /// </summary>
    public required string Description { get; set; }
}

/// <summary>
/// Result model for the created organization.
/// </summary>
public class CreateOrganizationResult
{
    /// <summary>
    /// Gets or sets the created organization.
    /// </summary>
    public OrganizationDto? Organization { get; set; }
}
