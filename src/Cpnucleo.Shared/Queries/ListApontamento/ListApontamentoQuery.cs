namespace Cpnucleo.Shared.Queries.ListApontamento;

public sealed record ListApontamentoQuery(bool GetDependencies = false) : BaseQuery, IRequest<ListApontamentoViewModel>;