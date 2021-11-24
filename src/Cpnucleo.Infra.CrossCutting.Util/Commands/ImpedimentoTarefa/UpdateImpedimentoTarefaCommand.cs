namespace Cpnucleo.Infra.CrossCutting.Util.Commands.ImpedimentoTarefa;

public class UpdateImpedimentoTarefaCommand : BaseCommand, IRequest<OperationResult>
{
    public ImpedimentoTarefaViewModel ImpedimentoTarefa { get; set; }
}