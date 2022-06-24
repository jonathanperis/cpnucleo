namespace Cpnucleo.Infra.CrossCutting.Shared.Queries.ImpedimentoTarefa;

public record GetImpedimentoTarefaQuery(Guid Id) : BaseQuery, IRequest<GetImpedimentoTarefaViewModel>;