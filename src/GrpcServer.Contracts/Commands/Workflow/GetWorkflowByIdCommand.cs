namespace GrpcServer.Contracts.Commands.Workflow;

/// <summary>
/// Command model for fetching a workflow by its unique identifier.
/// </summary>
public class GetWorkflowByIdCommand : ICommand<GetWorkflowByIdResult>
{
    /// <summary>
    /// Gets or sets the unique identifier for the workflow.
    /// </summary>
    public Guid Id { get; set; }
}

/// <summary>
/// Result model for the workflow fetched by its unique identifier.
/// </summary>
public class GetWorkflowByIdResult
{
    /// <summary>
    /// Gets or sets a value indicating whether the fetch was successful.
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// Gets or sets a message providing additional information about the result.
    /// </summary>
    public string Message { get; set; }
    
    /// <summary>
    /// Gets or sets the workflow details.
    /// </summary>
    public WorkflowDto? Workflow { get; set; }
}
