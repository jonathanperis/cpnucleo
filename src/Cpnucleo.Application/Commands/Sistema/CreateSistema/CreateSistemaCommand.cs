namespace Cpnucleo.Application.Commands.Sistema.CreateSistema;

public class CreateSistemaCommand : IRequest<OperationResult>
{
    public Guid Id { get; }
    public string Nome { get; }
    public string Descricao { get; }

    public CreateSistemaCommand()
    {

    }

    public CreateSistemaCommand(Guid id, string nome, string descricao)
    {
        Id = id;
        Nome = nome;
        Descricao = descricao;
    }
}
