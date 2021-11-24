namespace Cpnucleo.Infra.CrossCutting.Util.Commands.Apontamento;

public class UpdateApontamentoCommand : BaseCommand, IRequest<OperationResult>
{
    public ApontamentoViewModel Apontamento { get; set; }
}