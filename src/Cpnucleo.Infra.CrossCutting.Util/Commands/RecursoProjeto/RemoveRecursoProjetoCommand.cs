namespace Cpnucleo.Infra.CrossCutting.Util.Commands.RecursoProjeto;

public class RemoveRecursoProjetoCommand : IRequest<OperationResult>
{
    public Guid Id { get; set; }
}
