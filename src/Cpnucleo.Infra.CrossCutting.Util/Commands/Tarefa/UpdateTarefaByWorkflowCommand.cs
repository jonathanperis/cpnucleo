namespace Cpnucleo.Infra.CrossCutting.Util.Commands.Tarefa;

public class UpdateTarefaByWorkflowCommand : IRequest<OperationResult>
{
    public Guid Id { get; set; }
    public Guid IdWorkflow { get; set; }
}
