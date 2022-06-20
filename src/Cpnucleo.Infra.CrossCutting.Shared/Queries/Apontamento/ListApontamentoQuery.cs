namespace Cpnucleo.Infra.CrossCutting.Shared.Queries.Apontamento;

public record ListApontamentoQuery(bool GetDependencies = false) : BaseQuery, IRequest<ListApontamentoViewModel>;