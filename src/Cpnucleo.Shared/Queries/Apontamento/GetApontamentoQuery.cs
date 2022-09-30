namespace Cpnucleo.Shared.Queries.Apontamento;

public sealed record GetApontamentoQuery(Guid Id) : BaseQuery, IRequest<GetApontamentoViewModel>;