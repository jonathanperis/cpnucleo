namespace GrpcServer.Contracts.Commands.UserProject;

/// <summary>
/// Command model for updating an userProject.
/// </summary>
public class UpdateUserProjectCommand : ICommand<UpdateUserProjectResult>
{
    /// <summary>
    /// Gets or sets the unique identifier for the userProject.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier for the user.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier for the project.
    /// </summary>
    public Guid ProjectId { get; set; }
}

/// <summary>
/// Result model for the updated userProject.
/// </summary>
public class UpdateUserProjectResult
{
    /// <summary>
    /// Gets or sets a value indicating whether the update was successful.
    /// </summary>
    public bool Success { get; set; }
    /// <summary>
    /// Gets or sets a message providing additional information about the result.
    /// </summary>
    public string Message { get; set; }
}
