namespace Cpnucleo.Application.Commands.RecursoTarefa.UpdateRecursoTarefa;

public class UpdateRecursoTarefaCommand : IRequest<OperationResult>
{
    public UpdateRecursoTarefaCommand(Guid id, Guid idRecurso, Guid idTarefa, bool ativo)
    {
        Id = id;
        IdRecurso = idRecurso;
        IdTarefa = idTarefa;
        Ativo = ativo;
    }

    public Guid Id { get; }
    public Guid IdRecurso { get; }
    public Guid IdTarefa { get; }
    public bool Ativo { get; }
}
