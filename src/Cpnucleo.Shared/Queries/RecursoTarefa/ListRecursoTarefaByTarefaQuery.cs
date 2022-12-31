namespace Cpnucleo.Shared.Queries.RecursoTarefa;

public sealed record ListRecursoTarefaByTarefaQuery(Guid IdTarefa) : BaseQuery, IRequest<ListRecursoTarefaByTarefaViewModel>;