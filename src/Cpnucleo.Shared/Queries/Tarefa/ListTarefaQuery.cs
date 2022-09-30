namespace Cpnucleo.Shared.Queries.Tarefa;

public sealed record ListTarefaQuery(bool GetDependencies = false) : BaseQuery, IRequest<ListTarefaViewModel>;