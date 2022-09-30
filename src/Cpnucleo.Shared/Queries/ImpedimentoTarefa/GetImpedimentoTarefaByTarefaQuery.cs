namespace Cpnucleo.Shared.Queries.ImpedimentoTarefa;

public sealed record GetImpedimentoTarefaByTarefaQuery(Guid IdTarefa) : BaseQuery, IRequest<GetImpedimentoTarefaByTarefaViewModel>;