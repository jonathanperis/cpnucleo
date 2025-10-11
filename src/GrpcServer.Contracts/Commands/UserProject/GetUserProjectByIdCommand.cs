namespace GrpcServer.Contracts.Commands.UserProject;

/// <summary>
/// Command model for fetching an userProject by its unique identifier.
/// </summary>
public class GetUserProjectByIdCommand : ICommand<GetUserProjectByIdResult>
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
    /// Gets or sets a value indicating whether the fetch was successful.
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// Gets or sets a message providing additional information about the result.
    /// </summary>
    public string Message { get; set; }
    
    /// <summary>
    /// Gets or sets the userProject details.
    /// </summary>
    public UserProjectDto? UserProject { get; set; }
}
