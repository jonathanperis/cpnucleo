namespace Cpnucleo.Shared.Queries.TipoTarefa;

public record ListTipoTarefaQuery(bool GetDependencies = false) : BaseQuery, IRequest<ListTipoTarefaViewModel>;