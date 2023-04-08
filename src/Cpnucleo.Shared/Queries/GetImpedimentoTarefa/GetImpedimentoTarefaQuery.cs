namespace Cpnucleo.Shared.Queries.GetImpedimentoTarefa;

public sealed record GetImpedimentoTarefaQuery(Guid Id) : BaseQuery, IRequest<GetImpedimentoTarefaViewModel>;