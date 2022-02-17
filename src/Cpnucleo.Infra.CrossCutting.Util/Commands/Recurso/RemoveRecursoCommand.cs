namespace Cpnucleo.Infra.CrossCutting.Util.Commands.Recurso;

public class RemoveRecursoCommand : IRequest<OperationResult>
{
    public Guid Id { get; set; }
}
