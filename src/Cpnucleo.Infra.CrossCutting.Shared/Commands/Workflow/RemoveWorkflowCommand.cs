namespace Cpnucleo.Infra.CrossCutting.Shared.Commands.Workflow;

public class RemoveWorkflowCommand : BaseCommand, IRequest<OperationResult>
{
    public Guid Id { get; set; }
}
