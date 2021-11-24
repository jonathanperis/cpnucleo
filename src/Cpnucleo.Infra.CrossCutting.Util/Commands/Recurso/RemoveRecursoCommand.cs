namespace Cpnucleo.Infra.CrossCutting.Util.Commands.Recurso;

public class RemoveRecursoCommand : BaseCommand, IRequest<OperationResult>
{
    public Guid Id { get; set; }
}