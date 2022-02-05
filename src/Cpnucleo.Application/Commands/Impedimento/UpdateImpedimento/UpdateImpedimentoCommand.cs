namespace Cpnucleo.Application.Commands.Impedimento.UpdateImpedimento;

public class UpdateImpedimentoCommand : IRequest<OperationResult>
{
    public Guid Id { get; set; }
    public string Nome { get; set; }
}
