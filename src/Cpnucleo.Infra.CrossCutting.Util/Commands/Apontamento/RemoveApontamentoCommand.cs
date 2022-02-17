namespace Cpnucleo.Infra.CrossCutting.Util.Commands.Apontamento;

public class RemoveApontamentoCommand : IRequest<OperationResult>
{
    public Guid Id { get; set; }
}
