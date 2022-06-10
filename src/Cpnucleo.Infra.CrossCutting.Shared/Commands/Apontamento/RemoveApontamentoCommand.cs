namespace Cpnucleo.Infra.CrossCutting.Shared.Commands.Apontamento;

public class RemoveApontamentoCommand : BaseCommand, IRequest<OperationResult>
{
    public Guid Id { get; set; }
}
