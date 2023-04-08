namespace Cpnucleo.Shared.Queries.GetRecursoTarefa;

public sealed record GetRecursoTarefaQuery(Guid Id) : BaseQuery, IRequest<GetRecursoTarefaViewModel>;