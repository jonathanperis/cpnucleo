namespace Cpnucleo.Infra.CrossCutting.Util.Commands.Workflow;

public class RemoveWorkflowCommand : IRequest<OperationResult>
{
    public Guid Id { get; set; }
}
