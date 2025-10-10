namespace GrpcServer.Contracts.Commands.Assignment;

/// <summary>
/// Command model for listing assignments.
/// </summary>
public class ListAssignmentsCommand : ICommand<ListAssignmentsResult>
{
    /// <summary>
    /// Gets or sets the pagination parameters for the request.
    /// </summary>
    public required PaginationParams Pagination { get; set; }
}

/// <summary>
/// Result model for the list of assignments.
/// </summary>
public class ListAssignmentsResult
{
    /// <summary>
    /// Gets or sets the paginated result of assignments.
    /// </summary>
    public required PaginatedResult<AssignmentDto?> Result { get; set; }
}
