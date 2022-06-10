namespace Cpnucleo.Infra.CrossCutting.Shared.Commands.RecursoTarefa;

public class UpdateRecursoTarefaCommand : BaseCommand, IRequest<OperationResult>
{
    public Guid Id { get; set; }
    public Guid IdRecurso { get; set; }
    public Guid IdTarefa { get; set; }
}
