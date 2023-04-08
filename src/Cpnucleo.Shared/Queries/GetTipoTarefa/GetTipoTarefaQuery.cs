namespace Cpnucleo.Shared.Queries.GetTipoTarefa;

public sealed record GetTipoTarefaQuery(Guid Id) : BaseQuery, IRequest<GetTipoTarefaViewModel>;