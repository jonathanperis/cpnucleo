namespace GrpcServer.Contracts.Commands.Impediment;

/// <summary>
/// Command model for listing impediments.
/// </summary>
public class ListImpedimentsCommand
{
    /// <summary>
    /// Gets or sets the pagination parameters for the request.
    /// </summary>
    public required PaginationParams Pagination { get; set; }
}

/// <summary>
/// Result model for the list of impediments.
/// </summary>
public class ListImpedimentsResult
{
    /// <summary>
    /// Gets or sets the paginated result of impediments.
    /// </summary>
    public required PaginatedResult<ImpedimentDto?> Result { get; set; }
}
