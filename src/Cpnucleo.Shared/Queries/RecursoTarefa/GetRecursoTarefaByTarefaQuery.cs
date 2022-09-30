namespace Cpnucleo.Shared.Queries.RecursoTarefa;

public sealed record GetRecursoTarefaByTarefaQuery(Guid IdTarefa) : BaseQuery, IRequest<GetRecursoTarefaByTarefaViewModel>;