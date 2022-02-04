namespace Cpnucleo.Application.Queries.ImpedimentoTarefa.ListImpedimentoTarefa;

public class ListImpedimentoTarefaQuery : IRequest<ListImpedimentoTarefaViewModel>
{
    public bool GetDependencies { get; }

    public ListImpedimentoTarefaQuery(bool getDependencies)
    {
        GetDependencies = getDependencies;
    }
}
