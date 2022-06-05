namespace Cpnucleo.Infra.CrossCutting.Util.Commands.Impedimento;

public class UpdateImpedimentoCommand : BaseCommand, IRequest<OperationResult>
{
    public Guid Id { get; set; }
    public string Nome { get; set; }
}
