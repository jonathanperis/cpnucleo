namespace Cpnucleo.Infra.CrossCutting.Util.Commands.Impedimento.CreateImpedimento;

public class CreateImpedimentoResponse : BaseCommand
{
    public OperationResult Status { get; set; }

    public ImpedimentoViewModel Impedimento { get; set; }
}