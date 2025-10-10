namespace GrpcServer.Contracts.Commands.Project;

/// <summary>
/// Command model for updating a project.
/// </summary>
public class UpdateProjectCommand
{
    /// <summary>
    /// Gets or sets the unique identifier for the project.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the project.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier for the organization.
    /// </summary>
    public Guid OrganizationId { get; set; }
}

/// <summary>
/// Result model for the updated project.
/// </summary>
public class UpdateProjectResult
{
    /// <summary>
    /// Gets or sets a value indicating whether the update was successful.
    /// </summary>
    public bool Success { get; set; }
}
