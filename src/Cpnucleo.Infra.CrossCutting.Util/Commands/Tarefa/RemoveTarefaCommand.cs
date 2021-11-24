namespace Cpnucleo.Infra.CrossCutting.Util.Commands.Tarefa;

public class RemoveTarefaCommand : BaseCommand, IRequest<OperationResult>
{
    public Guid Id { get; set; }
}