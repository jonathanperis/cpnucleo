namespace Cpnucleo.Shared.Queries.ImpedimentoTarefa;

public record GetImpedimentoTarefaByTarefaQuery(Guid IdTarefa) : BaseQuery, IRequest<GetImpedimentoTarefaByTarefaViewModel>;