namespace Cpnucleo.Infra.CrossCutting.Util.Commands.ImpedimentoTarefa;

public class CreateImpedimentoTarefaCommand : BaseCommand, IRequest<OperationResult>
{
    public ImpedimentoTarefaViewModel ImpedimentoTarefa { get; set; }
}