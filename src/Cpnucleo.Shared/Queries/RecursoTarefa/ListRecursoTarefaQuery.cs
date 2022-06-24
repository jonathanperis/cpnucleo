namespace Cpnucleo.Shared.Queries.RecursoTarefa;

public record ListRecursoTarefaQuery(bool GetDependencies = false) : BaseQuery, IRequest<ListRecursoTarefaViewModel>;