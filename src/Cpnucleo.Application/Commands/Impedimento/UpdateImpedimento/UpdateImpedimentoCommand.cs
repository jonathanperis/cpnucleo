namespace Cpnucleo.Application.Commands.Impedimento.UpdateImpedimento;

public class UpdateImpedimentoCommand : IRequest<OperationResult>
{
    public Guid Id { get; }
    public string Nome { get; }
    public bool Ativo { get; }

    public UpdateImpedimentoCommand(Guid id, string nome, bool ativo)
    {
        Id = id;
        Nome = nome;
        Ativo = ativo;
    }
}
