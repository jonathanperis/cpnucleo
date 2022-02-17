namespace Cpnucleo.Infra.CrossCutting.Util.Commands.Tarefa;

public class RemoveTarefaCommand : IRequest<OperationResult>
{
    public Guid Id { get; set; }
}
