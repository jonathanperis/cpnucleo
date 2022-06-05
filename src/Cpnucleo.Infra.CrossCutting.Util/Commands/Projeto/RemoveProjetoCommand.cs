namespace Cpnucleo.Infra.CrossCutting.Util.Commands.Projeto;

public class RemoveProjetoCommand : BaseCommand, IRequest<OperationResult>
{
    public Guid Id { get; set; }
}
