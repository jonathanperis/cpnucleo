namespace Cpnucleo.Infra.CrossCutting.Shared.Commands.Workflow;

public class UpdateWorkflowCommand : BaseCommand, IRequest<OperationResult>
{
    public Guid Id { get; set; }
    public string Nome { get; set; }
    public int Ordem { get; set; }
}
