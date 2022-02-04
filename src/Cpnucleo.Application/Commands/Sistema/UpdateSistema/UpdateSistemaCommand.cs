namespace Cpnucleo.Application.Commands.Sistema.UpdateSistema;

public class UpdateSistemaCommand : IRequest<OperationResult>
{
    public Guid Id { get; }
    public string Nome { get; }
    public string Descricao { get; }
    public bool Ativo { get; }

    public UpdateSistemaCommand(Guid id, string nome, string descricao, bool ativo)
    {
        Id = id;
        Nome = nome;
        Descricao = descricao;
        Ativo = ativo;
    }
}
