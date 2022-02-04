namespace Cpnucleo.Application.Commands.RecursoProjeto.RemoveRecursoProjeto;

public class RemoveRecursoProjetoCommand : IRequest<OperationResult>
{
    public Guid Id { get; }

    public RemoveRecursoProjetoCommand(Guid id)
    {
        Id = id;
    }
}
