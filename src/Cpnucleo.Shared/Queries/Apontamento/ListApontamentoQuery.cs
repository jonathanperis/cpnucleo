namespace Cpnucleo.Shared.Queries.Apontamento;

public record ListApontamentoQuery(bool GetDependencies = false) : BaseQuery, IRequest<ListApontamentoViewModel>;