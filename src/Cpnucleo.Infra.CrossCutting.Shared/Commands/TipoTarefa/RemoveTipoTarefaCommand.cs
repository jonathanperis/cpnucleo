namespace Cpnucleo.Infra.CrossCutting.Shared.Commands.TipoTarefa;

public class RemoveTipoTarefaCommand : BaseCommand, IRequest<OperationResult>
{
    public Guid Id { get; set; }
}
