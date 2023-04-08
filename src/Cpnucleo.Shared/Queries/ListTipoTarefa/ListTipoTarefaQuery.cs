namespace Cpnucleo.Shared.Queries.ListTipoTarefa;

public sealed record ListTipoTarefaQuery(bool GetDependencies = false) : BaseQuery, IRequest<ListTipoTarefaViewModel>;