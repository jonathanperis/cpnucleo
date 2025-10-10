namespace GrpcServer.Contracts.Commands.UserProject;

/// <summary>
/// Command model for fetching an userProject by its unique identifier.
/// </summary>
public class GetUserProjectByIdCommand
{
    /// <summary>
    /// Gets or sets the unique identifier for the userProject.
    /// </summary>
    public Guid Id { get; set; }
}

/// <summary>
/// Result model for the userProject fetched by its unique identifier.
/// </summary>
public class GetUserProjectByIdResult
{
    /// <summary>
    /// Gets or sets the userProject details.
    /// </summary>
    public UserProjectDto? UserProject { get; set; }
}
