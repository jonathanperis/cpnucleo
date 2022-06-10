namespace Cpnucleo.Infra.CrossCutting.Shared.Commands.Projeto;

public class CreateProjetoCommand : BaseCommand, IRequest<OperationResult>
{
    public Guid Id { get; set; }
    public string Nome { get; set; }
    public Guid IdSistema { get; set; }
}
