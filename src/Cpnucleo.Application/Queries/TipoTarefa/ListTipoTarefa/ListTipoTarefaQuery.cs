namespace Cpnucleo.Application.Queries.TipoTarefa.ListTipoTarefa;

public class ListTipoTarefaQuery : IRequest<ListTipoTarefaViewModel>
{
    public bool GetDependencies { get; set; }
}
