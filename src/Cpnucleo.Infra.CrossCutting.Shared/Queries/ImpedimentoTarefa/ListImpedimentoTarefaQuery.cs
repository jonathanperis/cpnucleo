namespace Cpnucleo.Infra.CrossCutting.Shared.Queries.ImpedimentoTarefa;

public class ListImpedimentoTarefaQuery : BaseQuery, IRequest<ListImpedimentoTarefaViewModel>
{
    public bool GetDependencies { get; set; }
}
