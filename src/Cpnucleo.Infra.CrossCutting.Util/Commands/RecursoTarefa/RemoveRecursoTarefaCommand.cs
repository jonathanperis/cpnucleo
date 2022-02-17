namespace Cpnucleo.Infra.CrossCutting.Util.Commands.RecursoTarefa;

public class RemoveRecursoTarefaCommand : IRequest<OperationResult>
{
    public Guid Id { get; set; }
}
