namespace Cpnucleo.Infra.CrossCutting.Util.Commands.Tarefa;

public class UpdateByWorkflowCommand : BaseCommand
{
    public Guid IdTarefa { get; set; }

    public WorkflowViewModel Workflow { get; set; }
}