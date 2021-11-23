namespace Cpnucleo.Infra.CrossCutting.Util.Commands.Apontamento.CreateApontamento;

public class CreateApontamentoResponse : BaseCommand
{
    public OperationResult Status { get; set; }

    public ApontamentoViewModel Apontamento { get; set; }
}