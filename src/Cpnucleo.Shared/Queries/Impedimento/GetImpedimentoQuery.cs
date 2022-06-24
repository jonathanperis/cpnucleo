using Cpnucleo.Shared.Queries;

namespace Cpnucleo.Shared.Queries.Impedimento;

public record GetImpedimentoQuery(Guid Id) : BaseQuery, IRequest<GetImpedimentoViewModel>;