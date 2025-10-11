namespace GrpcServer.Contracts.Commands.Project;

/// <summary>
/// Command model for fetching a project by its unique identifier.
/// </summary>
public class GetProjectByIdCommand : ICommand<GetProjectByIdResult>
{
    /// <summary>
    /// Gets or sets the unique identifier for the project.
    /// </summary>
    public Guid Id { get; set; }
}

/// <summary>
/// Result model for the project fetched by its unique identifier.
/// </summary>
public class GetProjectByIdResult
{
    /// <summary>
    /// Gets or sets a value indicating whether the fetch was successful.
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// Gets or sets a message providing additional information about the result.
    /// </summary>
    public string Message { get; set; }
    
    /// <summary>
    /// Gets or sets the project details.
    /// </summary>
    public ProjectDto? Project { get; set; }
}