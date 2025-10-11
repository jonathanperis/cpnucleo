namespace GrpcServer.Contracts.Commands.UserProject;

/// <summary>
/// Command model for listing userProjects.
/// </summary>
public class ListUserProjectsCommand : ICommand<ListUserProjectsResult>
{
    /// <summary>
    /// Gets or sets the pagination parameters for the request.
    /// </summary>
    public required PaginationParams Pagination { get; set; }
}

/// <summary>
/// Result model for the list of userProjects.
/// </summary>
public class ListUserProjectsResult
{
    /// <summary>
    /// Gets or sets a value indicating whether the list was successful.
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// Gets or sets a message providing additional information about the result.
    /// </summary>
    public string Message { get; set; }
    
    /// <summary>
    /// Gets or sets the paginated result of userProjects.
    /// </summary>
    public required PaginatedResult<UserProjectDto?> Result { get; set; }
}
