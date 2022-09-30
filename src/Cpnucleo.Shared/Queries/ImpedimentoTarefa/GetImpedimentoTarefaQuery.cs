namespace Cpnucleo.Shared.Queries.ImpedimentoTarefa;

public sealed record GetImpedimentoTarefaQuery(Guid Id) : BaseQuery, IRequest<GetImpedimentoTarefaViewModel>;