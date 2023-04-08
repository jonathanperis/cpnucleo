namespace Cpnucleo.Shared.Queries.GetTarefa;

public sealed record GetTarefaQuery(Guid Id) : BaseQuery, IRequest<GetTarefaViewModel>;