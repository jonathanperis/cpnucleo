namespace Cpnucleo.Shared.Queries.TipoTarefa;

public sealed record ListTipoTarefaQuery(bool GetDependencies = false) : BaseQuery, IRequest<ListTipoTarefaViewModel>;