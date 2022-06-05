namespace Cpnucleo.Infra.CrossCutting.Util.Commands.Workflow;

public class RemoveWorkflowCommand : BaseCommand, IRequest<OperationResult>
{
    public Guid Id { get; set; }
}
