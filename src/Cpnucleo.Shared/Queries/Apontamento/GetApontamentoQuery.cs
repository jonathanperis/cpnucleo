namespace Cpnucleo.Shared.Queries.Apontamento;

public record GetApontamentoQuery(Guid Id) : BaseQuery, IRequest<GetApontamentoViewModel>;