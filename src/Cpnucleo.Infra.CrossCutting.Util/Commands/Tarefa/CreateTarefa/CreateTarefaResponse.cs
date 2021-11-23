namespace Cpnucleo.Infra.CrossCutting.Util.Commands.Tarefa.CreateTarefa;

public class CreateTarefaResponse : BaseCommand
{
    public OperationResult Status { get; set; }

    public TarefaViewModel Tarefa { get; set; }
}