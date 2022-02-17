namespace Cpnucleo.Infra.CrossCutting.Util.Queries.TipoTarefa;

public class GetTipoTarefaQuery : IRequest<GetTipoTarefaViewModel>
{
    public Guid Id { get; set; }
}
