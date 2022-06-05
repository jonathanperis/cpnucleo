namespace Cpnucleo.Infra.CrossCutting.Util.Commands.RecursoTarefa;

public class RemoveRecursoTarefaCommand : BaseCommand, IRequest<OperationResult>
{
    public Guid Id { get; set; }
}
