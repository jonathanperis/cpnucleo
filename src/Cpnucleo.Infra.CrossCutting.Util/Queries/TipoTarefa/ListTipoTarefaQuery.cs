namespace Cpnucleo.Infra.CrossCutting.Util.Queries.TipoTarefa;

public class ListTipoTarefaQuery : BaseQuery, IRequest<ListTipoTarefaViewModel>
{
    public bool GetDependencies { get; set; }
}
