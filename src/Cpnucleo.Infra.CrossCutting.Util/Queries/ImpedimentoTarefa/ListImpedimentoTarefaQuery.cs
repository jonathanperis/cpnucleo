namespace Cpnucleo.Infra.CrossCutting.Util.Queries.ImpedimentoTarefa;

public class ListImpedimentoTarefaQuery : IRequest<ListImpedimentoTarefaViewModel>
{
    public bool GetDependencies { get; set; }
}
