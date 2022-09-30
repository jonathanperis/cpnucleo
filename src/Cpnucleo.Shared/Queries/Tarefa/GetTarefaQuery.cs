namespace Cpnucleo.Shared.Queries.Tarefa;

public sealed record GetTarefaQuery(Guid Id) : BaseQuery, IRequest<GetTarefaViewModel>;