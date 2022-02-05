namespace Cpnucleo.Application.Commands.Tarefa.UpdateByWorkflow;

public class UpdateByWorkflowCommand : IRequest<OperationResult>
{
    public Guid Id { get; set; }
    public Guid IdWorkflow { get; set; }
}
