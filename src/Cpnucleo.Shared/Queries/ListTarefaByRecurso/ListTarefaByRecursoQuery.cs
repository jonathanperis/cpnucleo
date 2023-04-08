namespace Cpnucleo.Shared.Queries.ListTarefaByRecurso;

public sealed record ListTarefaByRecursoQuery(Guid IdRecurso) : BaseQuery, IRequest<ListTarefaByRecursoViewModel>;