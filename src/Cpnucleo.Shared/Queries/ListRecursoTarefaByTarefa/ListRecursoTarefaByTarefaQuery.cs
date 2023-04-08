namespace Cpnucleo.Shared.Queries.ListRecursoTarefaByTarefa;

public sealed record ListRecursoTarefaByTarefaQuery(Guid IdTarefa) : BaseQuery, IRequest<ListRecursoTarefaByTarefaViewModel>;