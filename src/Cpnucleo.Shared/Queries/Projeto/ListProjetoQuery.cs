namespace Cpnucleo.Shared.Queries.Projeto;

public sealed record ListProjetoQuery(bool GetDependencies = false) : BaseQuery, IRequest<ListProjetoViewModel>;