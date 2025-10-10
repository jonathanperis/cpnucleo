namespace GrpcServer.Contracts.Commands.Project;

/// <summary>
/// Command model for listing projects.
/// </summary>
public class ListProjectsCommand : ICommand<ListProjectsResult>
{
    /// <summary>
    /// Gets or sets the pagination parameters for the request.
    /// </summary>
    public required PaginationParams Pagination { get; set; }
}

/// <summary>
/// Result model for the list of projects.
/// </summary>
public class ListProjectsResult
{
    /// <summary>
    /// Gets or sets the paginated result of projects.
    /// </summary>
    public required PaginatedResult<ProjectDto?> Result { get; set; }
}
