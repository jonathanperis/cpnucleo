namespace Cpnucleo.Shared.Queries.TipoTarefa;

public sealed record GetTipoTarefaQuery(Guid Id) : BaseQuery, IRequest<GetTipoTarefaViewModel>;