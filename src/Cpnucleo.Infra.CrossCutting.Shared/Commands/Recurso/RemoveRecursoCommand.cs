namespace Cpnucleo.Infra.CrossCutting.Shared.Commands.Recurso;

public class RemoveRecursoCommand : BaseCommand, IRequest<OperationResult>
{
    public Guid Id { get; set; }
}
