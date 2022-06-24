namespace Cpnucleo.Shared.Queries.ImpedimentoTarefa;

public record ListImpedimentoTarefaQuery(bool GetDependencies = false) : BaseQuery, IRequest<ListImpedimentoTarefaViewModel>;