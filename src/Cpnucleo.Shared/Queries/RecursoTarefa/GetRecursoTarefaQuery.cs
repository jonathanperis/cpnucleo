namespace Cpnucleo.Shared.Queries.RecursoTarefa;

public sealed record GetRecursoTarefaQuery(Guid Id) : BaseQuery, IRequest<GetRecursoTarefaViewModel>;