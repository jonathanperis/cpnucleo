namespace Cpnucleo.Infra.CrossCutting.Shared.Queries.TipoTarefa;

public record ListTipoTarefaQuery(bool GetDependencies = false) : BaseQuery, IRequest<ListTipoTarefaViewModel>;