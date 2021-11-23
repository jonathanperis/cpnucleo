namespace Cpnucleo.Infra.CrossCutting.Util.Commands.Tarefa.UpdateByWorkflow;

public class UpdateByWorkflowCommand : BaseCommand
{
    public Guid IdTarefa { get; set; }

    public WorkflowViewModel Workflow { get; set; }
}