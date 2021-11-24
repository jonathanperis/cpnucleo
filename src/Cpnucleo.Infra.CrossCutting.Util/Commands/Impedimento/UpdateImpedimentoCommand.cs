namespace Cpnucleo.Infra.CrossCutting.Util.Commands.Impedimento;

public class UpdateImpedimentoCommand : BaseCommand, IRequest<OperationResult>
{
    public ImpedimentoViewModel Impedimento { get; set; }
}