namespace Cpnucleo.Application.Commands.Projeto.UpdateProjeto;

public class UpdateProjetoCommand : IRequest<OperationResult>
{
    public Guid Id { get; set; }
    public string Nome { get; set; }
    public Guid IdSistema { get; set; }
}
