namespace Cpnucleo.Infra.CrossCutting.Shared.Commands.TipoTarefa;

public class UpdateTipoTarefaCommand : BaseCommand, IRequest<OperationResult>
{
    public Guid Id { get; set; }
    public string Nome { get; set; }
    public string Image { get; set; }
}
