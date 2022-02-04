namespace Cpnucleo.Application.Commands.TipoTarefa.CreateTipoTarefa;

public class CreateTipoTarefaCommand : IRequest<OperationResult>
{
    public Guid Id { get; }
    public string Nome { get; }
    public string Image { get; }

    public CreateTipoTarefaCommand(Guid id, string nome, string image)
    {
        Id = id;
        Nome = nome;
        Image = image;
    }
}
