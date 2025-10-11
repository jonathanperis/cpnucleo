namespace GrpcServer.Contracts.Commands.Project;

/// <summary>
/// Command model for creating a new project.
/// </summary>
public class CreateProjectCommand : ICommand<CreateProjectResult>
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
/// Result model for the created project.
/// </summary>
public class CreateProjectResult
{
    /// <summary>
    /// Gets or sets a value indicating whether the creation was successful.
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// Gets or sets a message providing additional information about the result.
    /// </summary>
    public string Message { get; set; }
    
    /// <summary>
    /// Gets or sets the created project.
    /// </summary>
    public ProjectDto? Project { get; set; }
}
