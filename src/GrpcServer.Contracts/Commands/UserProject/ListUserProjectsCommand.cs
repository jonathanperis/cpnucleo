namespace GrpcServer.Contracts.Commands.UserProject;

/// <summary>
/// Command model for listing userProjects.
/// </summary>
public class ListUserProjectsCommand
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
    /// Gets or sets the paginated result of userProjects.
    /// </summary>
    public required PaginatedResult<UserProjectDto?> Result { get; set; }
}
