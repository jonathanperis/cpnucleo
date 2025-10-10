namespace GrpcServer.Contracts.Commands.AssignmentType;

/// <summary>
/// Command model for listing assignmentTypes.
/// </summary>
public class ListAssignmentTypesCommand
{
    /// <summary>
    /// Gets or sets the pagination parameters for the request.
    /// </summary>
    public required PaginationParams Pagination { get; set; }
}

/// <summary>
/// Result model for the list of assignmentTypes.
/// </summary>
public class ListAssignmentTypesResult
{
    /// <summary>
    /// Gets or sets the paginated result of assignmentTypes.
    /// </summary>
    public required PaginatedResult<AssignmentTypeDto?> Result { get; set; }
}
