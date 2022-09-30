namespace Cpnucleo.Shared.Queries.ImpedimentoTarefa;

public sealed record ListImpedimentoTarefaQuery(bool GetDependencies = false) : BaseQuery, IRequest<ListImpedimentoTarefaViewModel>;