namespace Cpnucleo.Application.Commands.Tarefa.RemoveTarefa;

public class RemoveTarefaCommand : IRequest<OperationResult>
{
    public Guid Id { get; }

    public RemoveTarefaCommand(Guid id)
    {
        Id = id;
    }
}
