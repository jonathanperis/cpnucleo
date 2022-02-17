namespace Cpnucleo.Infra.CrossCutting.Util.Commands.TipoTarefa;

public class UpdateTipoTarefaCommand : IRequest<OperationResult>
{
    public Guid Id { get; set; }
    public string Nome { get; set; }
    public string Image { get; set; }
}
