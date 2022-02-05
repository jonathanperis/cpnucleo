namespace Cpnucleo.Application.Commands.Projeto.CreateProjeto;

public class CreateProjetoCommand : IRequest<OperationResult>
{
    public Guid Id { get; set; }
    public string Nome { get; set; }
    public Guid IdSistema { get; set; }
}
