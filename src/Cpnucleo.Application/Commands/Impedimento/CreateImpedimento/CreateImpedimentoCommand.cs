namespace Cpnucleo.Application.Commands.Impedimento.CreateImpedimento;

public class CreateImpedimentoCommand : IRequest<OperationResult>
{
    public Guid Id { get; set; }
    public string Nome { get; set; }
}
