namespace Cpnucleo.Infra.CrossCutting.Util.Queries.TipoTarefa;

public class ListTipoTarefaQuery : BaseQuery, IRequest<IEnumerable<TipoTarefaViewModel>>
{
    public bool GetDependencies { get; set; }
}