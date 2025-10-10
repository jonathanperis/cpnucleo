namespace GrpcServer.Contracts.Commands.UserAssignment;

/// <summary>
/// Command model for listing userAssignments.
/// </summary>
public class ListUserAssignmentsCommand
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
    /// Gets or sets the paginated result of userAssignments.
    /// </summary>
    public required PaginatedResult<UserAssignmentDto?> Result { get; set; }
}
