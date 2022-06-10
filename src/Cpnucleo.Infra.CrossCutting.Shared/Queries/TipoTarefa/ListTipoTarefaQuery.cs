namespace Cpnucleo.Infra.CrossCutting.Shared.Queries.TipoTarefa;

public class ListTipoTarefaQuery : BaseQuery, IRequest<ListTipoTarefaViewModel>
{
    public bool GetDependencies { get; set; }
}
