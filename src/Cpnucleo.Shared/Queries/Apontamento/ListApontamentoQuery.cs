namespace Cpnucleo.Shared.Queries.Apontamento;

public sealed record ListApontamentoQuery(bool GetDependencies = false) : BaseQuery, IRequest<ListApontamentoViewModel>;