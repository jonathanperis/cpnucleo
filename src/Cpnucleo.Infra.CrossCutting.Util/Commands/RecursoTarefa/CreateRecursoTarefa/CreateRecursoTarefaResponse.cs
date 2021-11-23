namespace Cpnucleo.Infra.CrossCutting.Util.Commands.RecursoTarefa.CreateRecursoTarefa;

public class CreateRecursoTarefaResponse : BaseCommand
{
    public OperationResult Status { get; set; }

    public RecursoTarefaViewModel RecursoTarefa { get; set; }
}