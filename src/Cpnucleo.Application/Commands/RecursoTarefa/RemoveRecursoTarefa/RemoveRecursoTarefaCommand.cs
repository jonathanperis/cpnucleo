namespace Cpnucleo.Application.Commands.RecursoTarefa.RemoveRecursoTarefa;

public class RemoveRecursoTarefaCommand : IRequest<OperationResult>
{
    public Guid Id { get; set; }
}
