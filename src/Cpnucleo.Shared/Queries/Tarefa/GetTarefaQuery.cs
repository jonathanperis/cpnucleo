namespace Cpnucleo.Shared.Queries.Tarefa;

public record GetTarefaQuery(Guid Id) : BaseQuery, IRequest<GetTarefaViewModel>;