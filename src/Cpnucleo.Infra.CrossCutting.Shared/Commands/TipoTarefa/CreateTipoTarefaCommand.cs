namespace Cpnucleo.Infra.CrossCutting.Shared.Commands.TipoTarefa;

public class CreateTipoTarefaCommand : BaseCommand, IRequest<OperationResult>
{
    public Guid Id { get; set; }
    public string Nome { get; set; }
    public string Image { get; set; }
}
