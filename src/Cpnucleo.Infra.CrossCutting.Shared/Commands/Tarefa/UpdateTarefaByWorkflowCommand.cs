namespace Cpnucleo.Infra.CrossCutting.Shared.Commands.Tarefa;

public class UpdateTarefaByWorkflowCommand : BaseCommand, IRequest<OperationResult>
{
    public Guid Id { get; set; }
    public Guid IdWorkflow { get; set; }
}
