namespace Cpnucleo.Application.Commands.RecursoTarefa.UpdateRecursoTarefa;

public class UpdateRecursoTarefaCommand : IRequest<OperationResult>
{
    public Guid Id { get; set; }
    public Guid IdRecurso { get; set; }
    public Guid IdTarefa { get; set; }
    public bool Ativo { get; set; }
}
