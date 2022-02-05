namespace Cpnucleo.Application.Commands.Workflow.UpdateWorkflow;

public class UpdateWorkflowCommand : IRequest<OperationResult>
{
    public Guid Id { get; set; }
    public string Nome { get; set; }
    public int Ordem { get; set; }
}
