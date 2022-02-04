namespace Cpnucleo.Application.Commands.Workflow.UpdateWorkflow;

public class UpdateWorkflowCommand : IRequest<OperationResult>
{
    public Guid Id { get; }
    public string Nome { get; }
    public int Ordem { get; }
    public bool Ativo { get; }

    public UpdateWorkflowCommand(Guid id, string nome, int ordem, bool ativo)
    {
        Id = id;
        Nome = nome;
        Ordem = ordem;
        Ativo = ativo;
    }
}
