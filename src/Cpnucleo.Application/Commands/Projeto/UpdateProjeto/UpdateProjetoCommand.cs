namespace Cpnucleo.Application.Commands.Projeto.UpdateProjeto;

public class UpdateProjetoCommand : IRequest<OperationResult>
{
    public Guid Id { get; }
    public string Nome { get; }
    public bool Ativo { get; }
    public Guid IdSistema { get; set; }

    public UpdateProjetoCommand(Guid id, string nome, bool ativo, Guid idSistema)
    {
        Id = id;
        Nome = nome;
        Ativo = ativo;
        IdSistema = idSistema;
    }
}
