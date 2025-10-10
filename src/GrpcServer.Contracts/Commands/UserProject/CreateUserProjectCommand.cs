namespace GrpcServer.Contracts.Commands.UserProject;

/// <summary>
/// Command model for creating a new userProject.
/// </summary>
public class CreateUserProjectCommand
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
/// Result model for the created userProject.
/// </summary>
public class CreateUserProjectResult
{
    /// <summary>
    /// Gets or sets the created userProject.
    /// </summary>
    public UserProjectDto? UserProject { get; set; }
}
