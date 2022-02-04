namespace Cpnucleo.Application.Commands.RecursoTarefa.CreateRecursoTarefa;

public class CreateRecursoTarefaCommand : IRequest<OperationResult>
{
    public CreateRecursoTarefaCommand(Guid id, Guid idRecurso, Guid idTarefa)
    {
        Id = id;
        IdRecurso = idRecurso;
        IdTarefa = idTarefa;
    }

    public Guid Id { get; }
    public Guid IdRecurso { get; }
    public Guid IdTarefa { get; }
}
