using Cpnucleo.Shared.Queries;

namespace Cpnucleo.Shared.Queries.Tarefa;

public record ListTarefaQuery(bool GetDependencies = false) : BaseQuery, IRequest<ListTarefaViewModel>;