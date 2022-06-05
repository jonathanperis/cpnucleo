namespace Cpnucleo.Infra.CrossCutting.Util.Queries.TipoTarefa;

public class GetTipoTarefaQuery : BaseQuery, IRequest<GetTipoTarefaViewModel>
{
    public Guid Id { get; set; }
}
