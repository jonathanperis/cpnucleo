namespace Cpnucleo.Infra.CrossCutting.Util.Commands.Tarefa;

public class CreateTarefaCommand : BaseCommand, IRequest<OperationResult>
{
    public TarefaViewModel Tarefa { get; set; }
}