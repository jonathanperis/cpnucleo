namespace Cpnucleo.Infra.CrossCutting.Util.Queries.ImpedimentoTarefa.ListImpedimentoTarefa;

public class ListImpedimentoTarefaResponse : BaseQuery
{
    public OperationResult Status { get; set; }

    public IEnumerable<ImpedimentoTarefaViewModel> ImpedimentoTarefas { get; set; }
}