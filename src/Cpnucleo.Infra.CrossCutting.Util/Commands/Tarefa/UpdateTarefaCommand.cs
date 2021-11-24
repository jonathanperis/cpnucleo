namespace Cpnucleo.Infra.CrossCutting.Util.Commands.Tarefa;

public class UpdateTarefaCommand : BaseCommand, IRequest<OperationResult>
{
    public TarefaViewModel Tarefa { get; set; }
}