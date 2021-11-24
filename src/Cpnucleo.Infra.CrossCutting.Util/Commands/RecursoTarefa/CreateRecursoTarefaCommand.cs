namespace Cpnucleo.Infra.CrossCutting.Util.Commands.RecursoTarefa;

public class CreateRecursoTarefaCommand : BaseCommand, IRequest<OperationResult>
{
    public RecursoTarefaViewModel RecursoTarefa { get; set; }
}