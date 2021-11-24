namespace Cpnucleo.Infra.CrossCutting.Util.Commands.RecursoTarefa;

public class UpdateRecursoTarefaCommand : BaseCommand, IRequest<OperationResult>
{
    public RecursoTarefaViewModel RecursoTarefa { get; set; }
}