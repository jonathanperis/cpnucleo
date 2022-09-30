namespace Cpnucleo.Shared.Queries.RecursoTarefa;

public sealed record ListRecursoTarefaQuery(bool GetDependencies = false) : BaseQuery, IRequest<ListRecursoTarefaViewModel>;