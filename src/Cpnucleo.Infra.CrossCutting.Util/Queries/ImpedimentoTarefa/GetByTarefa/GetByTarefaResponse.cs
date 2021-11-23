namespace Cpnucleo.Infra.CrossCutting.Util.Queries.ImpedimentoTarefa.GetByTarefa;

public class GetByTarefaResponse : BaseQuery
{
    public OperationResult Status { get; set; }

    public IEnumerable<ImpedimentoTarefaViewModel> ImpedimentoTarefas { get; set; }
}