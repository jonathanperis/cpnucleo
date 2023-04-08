namespace Cpnucleo.Shared.Queries.ListImpedimentoTarefaByTarefa;

public sealed record ListImpedimentoTarefaByTarefaQuery(Guid IdTarefa) : BaseQuery, IRequest<ListImpedimentoTarefaByTarefaViewModel>;