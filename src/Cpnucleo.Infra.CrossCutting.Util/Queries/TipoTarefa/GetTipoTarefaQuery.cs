namespace Cpnucleo.Infra.CrossCutting.Util.Queries.TipoTarefa;

public class GetTipoTarefaQuery : BaseQuery, IRequest<TipoTarefaViewModel>
{
    public Guid Id { get; set; }
}