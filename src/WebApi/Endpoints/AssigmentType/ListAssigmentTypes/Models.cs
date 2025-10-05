namespace WebApi.Endpoints.AssignmentType.ListAssignmentTypes;

/// <summary>
/// Request model for listing assignmentTypes.
/// </summary>
public class Request
{
    /// <summary>
    /// Gets or sets the pagination parameters for the request.
    /// </summary>
    public required PaginationParams Pagination { get; set; }
}

/// <summary>
/// Response model for the list of assignmentTypes.
/// </summary>
public class Response
{
    /// <summary>
    /// Gets or sets the paginated result of assignmentTypes.
    /// </summary>
    public required PaginatedResult<AssignmentTypeDto?> Result { get; set; }
}
