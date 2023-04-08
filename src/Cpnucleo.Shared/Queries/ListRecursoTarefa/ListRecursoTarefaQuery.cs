namespace Cpnucleo.Shared.Queries.ListRecursoTarefa;

public sealed record ListRecursoTarefaQuery(bool GetDependencies = false) : BaseQuery, IRequest<ListRecursoTarefaViewModel>;