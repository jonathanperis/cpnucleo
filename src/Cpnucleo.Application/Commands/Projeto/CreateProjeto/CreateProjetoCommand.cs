namespace Cpnucleo.Application.Commands.Projeto.CreateProjeto;

public class CreateProjetoCommand : IRequest<OperationResult>
{
    public Guid Id { get; }
    public string Nome { get; }
    public Guid IdSistema { get; }

    public CreateProjetoCommand(Guid id, string nome, Guid idSistema)
    {
        Id = id;
        Nome = nome;
        IdSistema = idSistema;
    }
}
