namespace Cpnucleo.Shared.Queries.ListTarefa;

public sealed record ListTarefaQuery(bool GetDependencies = false) : BaseQuery, IRequest<ListTarefaViewModel>;