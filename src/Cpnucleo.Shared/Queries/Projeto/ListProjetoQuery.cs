namespace Cpnucleo.Shared.Queries.Projeto;

public record ListProjetoQuery(bool GetDependencies = false) : BaseQuery, IRequest<ListProjetoViewModel>;