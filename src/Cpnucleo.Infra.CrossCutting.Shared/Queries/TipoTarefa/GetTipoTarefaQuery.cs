namespace Cpnucleo.Infra.CrossCutting.Shared.Queries.TipoTarefa;

public record GetTipoTarefaQuery(Guid Id) : BaseQuery, IRequest<GetTipoTarefaViewModel>;