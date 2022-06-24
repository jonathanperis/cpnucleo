namespace Cpnucleo.Shared.Queries.TipoTarefa;

public record GetTipoTarefaQuery(Guid Id) : BaseQuery, IRequest<GetTipoTarefaViewModel>;