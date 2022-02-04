namespace Cpnucleo.Application.Commands.TipoTarefa.UpdateTipoTarefa;

public class UpdateTipoTarefaCommand : IRequest<OperationResult>
{
    public Guid Id { get; }
    public string Nome { get; }
    public string Image { get; }
    public bool Ativo { get; }

    public UpdateTipoTarefaCommand(Guid id, string nome, string image, bool ativo)
    {
        Id = id;
        Nome = nome;
        Image = image;
        Ativo = ativo;
    }
}
