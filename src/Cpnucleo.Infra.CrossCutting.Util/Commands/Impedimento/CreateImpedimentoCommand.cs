namespace Cpnucleo.Infra.CrossCutting.Util.Commands.Impedimento;

public class CreateImpedimentoCommand : IRequest<OperationResult>
{
    public Guid Id { get; set; }
    public string Nome { get; init; }
}
