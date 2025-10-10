namespace GrpcServer.Contracts.Commands.Workflow;

/// <summary>
/// Command model for creating a new workflow.
/// </summary>
public class CreateWorkflowCommand : ICommand<CreateWorkflowResult>
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
/// Result model for the created workflow.
/// </summary>
public class CreateWorkflowResult
{
    /// <summary>
    /// Gets or sets the created workflow.
    /// </summary>
    public WorkflowDto? Workflow { get; set; }
}
