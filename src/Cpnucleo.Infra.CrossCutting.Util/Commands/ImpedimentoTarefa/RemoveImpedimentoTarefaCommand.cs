namespace Cpnucleo.Infra.CrossCutting.Util.Commands.ImpedimentoTarefa;

public class RemoveImpedimentoTarefaCommand : BaseCommand, IRequest<OperationResult>
{
    public Guid Id { get; set; }
}