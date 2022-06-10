namespace Cpnucleo.Infra.CrossCutting.Shared.Queries.TipoTarefa;

public class GetTipoTarefaQuery : BaseQuery, IRequest<GetTipoTarefaViewModel>
{
    public Guid Id { get; set; }
}
