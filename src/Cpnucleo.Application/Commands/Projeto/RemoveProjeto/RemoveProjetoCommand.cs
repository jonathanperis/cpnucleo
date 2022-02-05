namespace Cpnucleo.Application.Commands.Projeto.RemoveProjeto;

public class RemoveProjetoCommand : IRequest<OperationResult>
{
    public Guid Id { get; set; }
}
