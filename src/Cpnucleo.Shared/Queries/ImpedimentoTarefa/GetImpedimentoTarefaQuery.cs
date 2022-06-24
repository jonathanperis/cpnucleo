using Cpnucleo.Shared.Queries;

namespace Cpnucleo.Shared.Queries.ImpedimentoTarefa;

public record GetImpedimentoTarefaQuery(Guid Id) : BaseQuery, IRequest<GetImpedimentoTarefaViewModel>;