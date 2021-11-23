namespace Cpnucleo.Infra.CrossCutting.Util.Queries.ImpedimentoTarefa.GetImpedimentoTarefa;

public class GetImpedimentoTarefaResponse : BaseQuery
{
    public OperationResult Status { get; set; }

    public ImpedimentoTarefaViewModel ImpedimentoTarefa { get; set; }
}