namespace Cpnucleo.Shared.Queries.Tarefa;

public sealed record ListTarefaByRecursoQuery(Guid IdRecurso) : BaseQuery, IRequest<ListTarefaByRecursoViewModel>;