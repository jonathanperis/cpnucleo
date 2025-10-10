namespace GrpcServer.Contracts.Commands.UserProject;

/// <summary>
/// Command model for removing an userProject.
/// </summary>
public class RemoveUserProjectCommand : ICommand<RemoveUserProjectResult>
{
    /// <summary>
    /// Gets or sets the unique identifiers for the userProjects.
    /// </summary>
    public required List<Guid> Ids { get; set; }
}

/// <summary>
/// Result model for the removal of an userProject.
/// </summary>
public class RemoveUserProjectResult
{
    /// <summary>
    /// Gets or sets a value indicating whether the removal was successful.
    /// </summary>
    public bool Success { get; set; }
}
