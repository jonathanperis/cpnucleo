namespace Cpnucleo.Application.Commands.Apontamento.RemoveApontamento;

public class RemoveApontamentoCommand : IRequest<OperationResult>
{
    public Guid Id { get; }

    public RemoveApontamentoCommand(Guid id)
    {
        Id = id;
    }
}
