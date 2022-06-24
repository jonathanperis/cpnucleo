namespace Cpnucleo.Shared.Queries.Tarefa;

public record GetTarefaByRecursoQuery(Guid IdRecurso) : BaseQuery, IRequest<GetTarefaByRecursoViewModel>;