namespace Cpnucleo.Infra.CrossCutting.Util.Commands.Impedimento;

public class CreateImpedimentoCommand : BaseCommand, IRequest<OperationResult>
{
    public ImpedimentoViewModel Impedimento { get; set; }
}