namespace Cpnucleo.Infra.CrossCutting.Shared.Queries.Tarefa;

public record GetTarefaQuery(Guid Id) : BaseQuery, IRequest<GetTarefaViewModel>;