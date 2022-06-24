namespace Cpnucleo.Infra.CrossCutting.Shared.Queries.Apontamento;

public record GetApontamentoQuery(Guid Id) : BaseQuery, IRequest<GetApontamentoViewModel>;