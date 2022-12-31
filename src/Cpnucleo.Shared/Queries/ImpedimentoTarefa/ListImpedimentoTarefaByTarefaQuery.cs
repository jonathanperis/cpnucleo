namespace Cpnucleo.Shared.Queries.ImpedimentoTarefa;

public sealed record ListImpedimentoTarefaByTarefaQuery(Guid IdTarefa) : BaseQuery, IRequest<ListImpedimentoTarefaByTarefaViewModel>;