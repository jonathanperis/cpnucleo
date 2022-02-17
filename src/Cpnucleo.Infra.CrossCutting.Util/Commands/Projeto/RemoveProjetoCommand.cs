namespace Cpnucleo.Infra.CrossCutting.Util.Commands.Projeto;

public class RemoveProjetoCommand : IRequest<OperationResult>
{
    public Guid Id { get; set; }
}
