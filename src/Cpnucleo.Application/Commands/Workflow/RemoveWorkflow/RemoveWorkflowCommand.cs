namespace Cpnucleo.Application.Commands.Workflow.RemoveWorkflow;

public class RemoveWorkflowCommand : IRequest<OperationResult>
{
    public Guid Id { get; }

    public RemoveWorkflowCommand(Guid id)
    {
        Id = id;
    }
}
