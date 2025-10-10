namespace GrpcServer.Contracts.Commands.AssignmentImpediment;

/// <summary>
/// Command model for listing assignmentImpediments.
/// </summary>
public class ListAssignmentImpedimentsCommand : ICommand<ListAssignmentImpedimentsResult>
{
    /// <summary>
    /// Gets or sets the pagination parameters for the request.
    /// </summary>
    public required PaginationParams Pagination { get; set; }
}

/// <summary>
/// Result model for the list of assignmentImpediments.
/// </summary>
public class ListAssignmentImpedimentsResult
{
    /// <summary>
    /// Gets or sets the paginated result of assignmentImpediments.
    /// </summary>
    public required PaginatedResult<AssignmentImpedimentDto?> Result { get; set; }
}
