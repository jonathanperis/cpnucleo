namespace Cpnucleo.Application.Queries.TipoTarefa.ListTipoTarefa;

public class ListTipoTarefaQuery : IRequest<ListTipoTarefaViewModel>
{
    public bool GetDependencies { get; }

    public ListTipoTarefaQuery(bool getDependencies)
    {
        GetDependencies = getDependencies;
    }
}
