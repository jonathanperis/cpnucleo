namespace GrpcServer.Contracts.Commands.Project;

/// <summary>
/// Command model for removing a project.
/// </summary>
public class RemoveProjectCommand
{
    /// <summary>
    /// Gets or sets the unique identifiers for the projects.
    /// </summary>
    public required List<Guid> Ids { get; set; }
}

/// <summary>
/// Result model for the removal of a project.
/// </summary>
public class RemoveProjectResult
{
    /// <summary>
    /// Gets or sets a value indicating whether the removal was successful.
    /// </summary>
    public bool Success { get; set; }
}
