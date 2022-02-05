namespace Cpnucleo.Application.Commands.TipoTarefa.CreateTipoTarefa;

public class CreateTipoTarefaCommand : IRequest<OperationResult>
{
    public Guid Id { get; set; }
    public string Nome { get; set; }
    public string Image { get; set; }
}
