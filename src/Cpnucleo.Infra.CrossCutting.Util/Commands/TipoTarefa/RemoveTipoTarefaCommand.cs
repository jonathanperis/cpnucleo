namespace Cpnucleo.Infra.CrossCutting.Util.Commands.TipoTarefa;

public class RemoveTipoTarefaCommand : BaseCommand, IRequest<OperationResult>
{
    public Guid Id { get; set; }
}
