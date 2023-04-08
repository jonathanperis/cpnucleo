namespace Cpnucleo.Shared.Queries.ListProjeto;

public sealed record ListProjetoQuery(bool GetDependencies = false) : BaseQuery, IRequest<ListProjetoViewModel>;