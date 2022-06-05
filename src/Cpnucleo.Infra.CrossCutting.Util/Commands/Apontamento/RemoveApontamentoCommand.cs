namespace Cpnucleo.Infra.CrossCutting.Util.Commands.Apontamento;

public class RemoveApontamentoCommand : BaseCommand, IRequest<OperationResult>
{
    public Guid Id { get; set; }
}
