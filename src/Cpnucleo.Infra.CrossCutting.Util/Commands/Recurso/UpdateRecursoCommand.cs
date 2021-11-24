namespace Cpnucleo.Infra.CrossCutting.Util.Commands.Recurso;

public class UpdateRecursoCommand : BaseCommand, IRequest<OperationResult>
{
    public RecursoViewModel Recurso { get; set; }
}