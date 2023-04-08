namespace Cpnucleo.Shared.Queries.GetApontamento;

public sealed record GetApontamentoQuery(Guid Id) : BaseQuery, IRequest<GetApontamentoViewModel>;