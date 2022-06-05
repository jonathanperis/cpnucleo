namespace Cpnucleo.Infra.CrossCutting.Util.Commands.RecursoTarefa;

public class UpdateRecursoTarefaCommand : BaseCommand, IRequest<OperationResult>
{
    public Guid Id { get; set; }
    public Guid IdRecurso { get; set; }
    public Guid IdTarefa { get; set; }
}
