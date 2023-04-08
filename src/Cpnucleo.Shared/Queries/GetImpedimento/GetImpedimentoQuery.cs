namespace Cpnucleo.Shared.Queries.GetImpedimento;

public sealed record GetImpedimentoQuery(Guid Id) : BaseQuery, IRequest<GetImpedimentoViewModel>;