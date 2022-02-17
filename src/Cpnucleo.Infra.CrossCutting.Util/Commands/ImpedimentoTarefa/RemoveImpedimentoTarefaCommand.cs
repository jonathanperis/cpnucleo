namespace Cpnucleo.Infra.CrossCutting.Util.Commands.ImpedimentoTarefa;

public class RemoveImpedimentoTarefaCommand : IRequest<OperationResult>
{
    public Guid Id { get; set; }
}
