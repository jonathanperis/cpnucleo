namespace Cpnucleo.Infra.CrossCutting.Util.Commands.ImpedimentoTarefa.CreateImpedimentoTarefa;

public class CreateImpedimentoTarefaResponse : BaseCommand
{
    public OperationResult Status { get; set; }

    public ImpedimentoTarefaViewModel ImpedimentoTarefa { get; set; }
}