namespace Cpnucleo.Infra.CrossCutting.Util.Commands.Workflow;

public class UpdateWorkflowCommand : BaseCommand, IRequest<OperationResult>
{
    public WorkflowViewModel Workflow { get; set; }
}