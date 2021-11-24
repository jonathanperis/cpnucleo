namespace Cpnucleo.Infra.CrossCutting.Util.Queries.ImpedimentoTarefa;

public class ListImpedimentoTarefaQuery : BaseQuery, IRequest<IEnumerable<ImpedimentoTarefaViewModel>>
{
    public bool GetDependencies { get; set; }
}