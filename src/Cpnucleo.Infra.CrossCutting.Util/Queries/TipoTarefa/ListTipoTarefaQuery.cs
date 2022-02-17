namespace Cpnucleo.Infra.CrossCutting.Util.Queries.TipoTarefa;

public class ListTipoTarefaQuery : IRequest<ListTipoTarefaViewModel>
{
    public bool GetDependencies { get; set; }
}
