namespace Cpnucleo.Application.Commands.Tarefa.UpdateTarefaByWorkflow;

public class UpdateTarefaByWorkflowCommand : IRequest<OperationResult>
{
    public Guid Id { get; set; }
    public Guid IdWorkflow { get; set; }
}
