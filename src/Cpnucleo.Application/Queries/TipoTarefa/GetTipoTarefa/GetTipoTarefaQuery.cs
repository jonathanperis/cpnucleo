namespace Cpnucleo.Application.Queries.TipoTarefa.GetTipoTarefa;

public class GetTipoTarefaQuery : IRequest<GetTipoTarefaViewModel>
{
    public Guid Id { get; }

    public GetTipoTarefaQuery(Guid id)
    {
        Id = id;
    }
}
