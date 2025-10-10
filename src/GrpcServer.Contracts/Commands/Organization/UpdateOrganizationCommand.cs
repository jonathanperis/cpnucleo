namespace GrpcServer.Contracts.Commands.Organization;

/// <summary>
/// Command model for updating an organization.
/// </summary>
public class UpdateOrganizationCommand
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
/// Result model for the updated organization.
/// </summary>
public class UpdateOrganizationResult
{
    /// <summary>
    /// Gets or sets a value indicating whether the update was successful.
    /// </summary>
    public bool Success { get; set; }
}
