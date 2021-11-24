namespace Cpnucleo.Infra.CrossCutting.Util.Commands.Recurso;

public class CreateRecursoCommand : BaseCommand, IRequest<OperationResult>
{
    public RecursoViewModel Recurso { get; set; }
}