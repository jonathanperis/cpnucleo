namespace Cpnucleo.Infra.CrossCutting.Shared.Queries.Tarefa;

public record GetTarefaByRecursoQuery(Guid IdRecurso) : BaseQuery, IRequest<GetTarefaByRecursoViewModel>;