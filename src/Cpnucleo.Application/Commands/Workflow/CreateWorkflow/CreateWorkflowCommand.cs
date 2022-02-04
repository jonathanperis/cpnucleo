namespace Cpnucleo.Application.Commands.Workflow.CreateWorkflow;

public class CreateWorkflowCommand : IRequest<OperationResult>
{
    public Guid Id { get; }
    public string Nome { get; }
    public int Ordem { get; }

    public CreateWorkflowCommand(Guid id, string nome, int ordem)
    {
        Id = id;
        Nome = nome;
        Ordem = ordem;
    }
}
