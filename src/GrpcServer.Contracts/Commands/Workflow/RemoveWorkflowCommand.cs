namespace GrpcServer.Contracts.Commands.Workflow;

/// <summary>
/// Command model for removing a workflow.
/// </summary>
public class RemoveWorkflowCommand : ICommand<RemoveWorkflowResult>
{
    /// <summary>
    /// Gets or sets the unique identifiers for the workflows.
    /// </summary>
    public required List<Guid> Ids { get; set; }
}

/// <summary>
/// Result model for the removal of a workflow.
/// </summary>
public class RemoveWorkflowResult
{
    /// <summary>
    /// Gets or sets a value indicating whether the removal was successful.
    /// </summary>
    public bool Success { get; set; }
}
