namespace Cpnucleo.Infra.CrossCutting.Shared.Commands.Impedimento;

public class CreateImpedimentoCommand : BaseCommand, IRequest<OperationResult>
{
    public Guid Id { get; set; }
    public string Nome { get; init; }
}
