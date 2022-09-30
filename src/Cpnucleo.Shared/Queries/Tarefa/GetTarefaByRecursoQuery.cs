namespace Cpnucleo.Shared.Queries.Tarefa;

public sealed record GetTarefaByRecursoQuery(Guid IdRecurso) : BaseQuery, IRequest<GetTarefaByRecursoViewModel>;