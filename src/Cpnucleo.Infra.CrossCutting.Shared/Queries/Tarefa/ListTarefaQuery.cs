namespace Cpnucleo.Infra.CrossCutting.Shared.Queries.Tarefa;

public record ListTarefaQuery(bool GetDependencies = false) : BaseQuery, IRequest<ListTarefaViewModel>;