namespace GrpcServer.Contracts.Commands.UserAssignment;

/// <summary>
/// Command model for listing userAssignments.
/// </summary>
public class ListUserAssignmentsCommand : ICommand<ListUserAssignmentsResult>
{
    /// <summary>
    /// Gets or sets the pagination parameters for the request.
    /// </summary>
    public required PaginationParams Pagination { get; set; }
}

/// <summary>
/// Result model for the list of userAssignments.
/// </summary>
public class ListUserAssignmentsResult
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
    /// Gets or sets the paginated result of userAssignments.
    /// </summary>
    public required PaginatedResult<UserAssignmentDto?> Result { get; set; }
}
