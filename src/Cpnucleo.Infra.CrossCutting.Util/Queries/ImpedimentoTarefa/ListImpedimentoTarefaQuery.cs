namespace Cpnucleo.Infra.CrossCutting.Util.Queries.ImpedimentoTarefa;

public class ListImpedimentoTarefaQuery : BaseQuery, IRequest<ListImpedimentoTarefaViewModel>
{
    public bool GetDependencies { get; set; }
}
