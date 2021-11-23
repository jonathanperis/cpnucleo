namespace Cpnucleo.Infra.CrossCutting.Util.Commands.Workflow.CreateWorkflow;

public class CreateWorkflowResponse : BaseCommand
{
    public OperationResult Status { get; set; }

    public WorkflowViewModel Workflow { get; set; }
}