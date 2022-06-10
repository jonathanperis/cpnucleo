namespace Cpnucleo.Infra.CrossCutting.Shared.Commands.RecursoProjeto;

public class RemoveRecursoProjetoCommand : BaseCommand, IRequest<OperationResult>
{
    public Guid Id { get; set; }
}
