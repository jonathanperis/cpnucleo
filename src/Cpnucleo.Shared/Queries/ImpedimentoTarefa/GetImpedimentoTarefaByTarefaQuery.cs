namespace Cpnucleo.Infra.CrossCutting.Shared.Queries.ImpedimentoTarefa;

public record GetImpedimentoTarefaByTarefaQuery(Guid IdTarefa) : BaseQuery, IRequest<GetImpedimentoTarefaByTarefaViewModel>;