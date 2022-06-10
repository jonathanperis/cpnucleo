namespace Cpnucleo.Infra.CrossCutting.Shared.Commands.RecursoTarefa;

public class RemoveRecursoTarefaCommand : BaseCommand, IRequest<OperationResult>
{
    public Guid Id { get; set; }
}
