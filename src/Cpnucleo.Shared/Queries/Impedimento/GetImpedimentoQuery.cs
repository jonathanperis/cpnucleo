namespace Cpnucleo.Shared.Queries.Impedimento;

public sealed record GetImpedimentoQuery(Guid Id) : BaseQuery, IRequest<GetImpedimentoViewModel>;