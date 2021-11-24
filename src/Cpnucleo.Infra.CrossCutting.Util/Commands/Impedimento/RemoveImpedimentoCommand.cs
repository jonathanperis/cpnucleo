namespace Cpnucleo.Infra.CrossCutting.Util.Commands.Impedimento;

public class RemoveImpedimentoCommand : BaseCommand, IRequest<OperationResult>
{
    public Guid Id { get; set; }
}