namespace Cpnucleo.Application.Commands.Workflow.CreateWorkflow;

public class CreateWorkflowCommand : IRequest<OperationResult>
{
    public Guid Id { get; set; }
    public string Nome { get; set; }
    public int Ordem { get; set; }
}
