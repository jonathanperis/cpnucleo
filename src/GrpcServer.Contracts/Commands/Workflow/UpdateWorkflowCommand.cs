namespace GrpcServer.Contracts.Commands.Workflow;

/// <summary>
/// Command model for updating a workflow.
/// </summary>
public class UpdateWorkflowCommand : ICommand<UpdateWorkflowResult>
{
    /// <summary>
    /// Gets or sets the unique identifier for the workflow.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the workflow.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Gets or sets the order of the workflow.
    /// </summary>
    public int Order { get; set; }
}

/// <summary>
/// Result model for the updated workflow.
/// </summary>
public class UpdateWorkflowResult
{
    /// <summary>
    /// Gets or sets a value indicating whether the update was successful.
    /// </summary>
    public bool Success { get; set; }
    
    /// <summary>
    /// Gets or sets a message providing additional information about the result.
    /// </summary>
    public string Message { get; set; }
}
