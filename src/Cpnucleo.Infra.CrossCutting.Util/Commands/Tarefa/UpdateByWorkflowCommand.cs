namespace Cpnucleo.Infra.CrossCutting.Util.Commands.Tarefa;

public class UpdateByWorkflowCommand : BaseCommand, IRequest<OperationResult>
{
    public Guid IdTarefa { get; set; }

    public WorkflowViewModel Workflow { get; set; }
}