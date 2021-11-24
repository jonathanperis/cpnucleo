namespace Cpnucleo.Infra.CrossCutting.Util.Commands.Workflow;

public class CreateWorkflowCommand : BaseCommand, IRequest<OperationResult>
{
    public WorkflowViewModel Workflow { get; set; }
}