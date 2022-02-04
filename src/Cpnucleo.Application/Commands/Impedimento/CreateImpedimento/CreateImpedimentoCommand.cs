namespace Cpnucleo.Application.Commands.Impedimento.CreateImpedimento;

public class CreateImpedimentoCommand : IRequest<OperationResult>
{
    public Guid Id { get; }
    public string Nome { get; }

    public CreateImpedimentoCommand(Guid id, string nome)
    {
        Id = id;
        Nome = nome;
    }
}
