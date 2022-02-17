namespace Cpnucleo.Infra.CrossCutting.Util.Commands.Projeto;

public class CreateProjetoCommand : IRequest<OperationResult>
{
    public Guid Id { get; set; }
    public string Nome { get; set; }
    public Guid IdSistema { get; set; }
}
