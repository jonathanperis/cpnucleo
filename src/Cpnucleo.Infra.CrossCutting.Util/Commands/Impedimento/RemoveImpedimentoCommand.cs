namespace Cpnucleo.Infra.CrossCutting.Util.Commands.Impedimento;

public class RemoveImpedimentoCommand : IRequest<OperationResult>
{
    public Guid Id { get; set; }
}
