namespace Cpnucleo.Application.Queries.ImpedimentoTarefa.ListImpedimentoTarefa;

public class ListImpedimentoTarefaQuery : IRequest<ListImpedimentoTarefaViewModel>
{
    public bool GetDependencies { get; set; }
}
