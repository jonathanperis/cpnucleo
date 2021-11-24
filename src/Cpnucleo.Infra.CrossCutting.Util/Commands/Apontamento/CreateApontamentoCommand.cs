namespace Cpnucleo.Infra.CrossCutting.Util.Commands.Apontamento;

public class CreateApontamentoCommand : BaseCommand, IRequest<OperationResult>
{
    public ApontamentoViewModel Apontamento { get; set; }
}