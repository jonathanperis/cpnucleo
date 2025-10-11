namespace GrpcServer.Contracts.Commands.Workflow;

/// <summary>
/// Command model for listing workflows.
/// </summary>
public class ListWorkflowsCommand : ICommand<ListWorkflowsResult>
{
    /// <summary>
    /// Gets or sets the pagination parameters for the request.
    /// </summary>
    public required PaginationParams Pagination { get; set; }
}

/// <summary>
/// Result model for the list of workflows.
/// </summary>
public class ListWorkflowsResult
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
    /// Gets or sets the paginated result of workflows.
    /// </summary>
    public required PaginatedResult<WorkflowDto?> Result { get; set; }
}
