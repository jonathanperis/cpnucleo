namespace Cpnucleo.Infra.CrossCutting.Util.Commands.RecursoProjeto;

public class RemoveRecursoProjetoCommand : BaseCommand, IRequest<OperationResult>
{
    public Guid Id { get; set; }
}
