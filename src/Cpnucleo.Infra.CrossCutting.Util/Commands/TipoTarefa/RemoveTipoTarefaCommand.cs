namespace Cpnucleo.Infra.CrossCutting.Util.Commands.TipoTarefa;

public class RemoveTipoTarefaCommand : IRequest<OperationResult>
{
    public Guid Id { get; set; }
}
