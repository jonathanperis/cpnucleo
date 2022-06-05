namespace Cpnucleo.Infra.CrossCutting.Util.Commands.Workflow;

public class CreateWorkflowCommand : BaseCommand, IRequest<OperationResult>
{
    public Guid Id { get; set; }
    public string Nome { get; set; }
    public int Ordem { get; set; }
}
