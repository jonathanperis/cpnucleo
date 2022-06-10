namespace Cpnucleo.Infra.CrossCutting.Shared.Commands.Impedimento;

public class RemoveImpedimentoCommand : BaseCommand, IRequest<OperationResult>
{
    public Guid Id { get; set; }
}
