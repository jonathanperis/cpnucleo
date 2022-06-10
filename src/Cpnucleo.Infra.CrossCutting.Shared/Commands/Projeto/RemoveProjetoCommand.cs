namespace Cpnucleo.Infra.CrossCutting.Shared.Commands.Projeto;

public class RemoveProjetoCommand : BaseCommand, IRequest<OperationResult>
{
    public Guid Id { get; set; }
}
