namespace Cpnucleo.Shared.Queries.ListImpedimentoTarefa;

public sealed record ListImpedimentoTarefaQuery(bool GetDependencies = false) : BaseQuery, IRequest<ListImpedimentoTarefaViewModel>;