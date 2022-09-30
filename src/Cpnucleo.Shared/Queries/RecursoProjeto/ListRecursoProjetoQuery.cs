namespace Cpnucleo.Shared.Queries.RecursoProjeto;

public sealed record ListRecursoProjetoQuery(bool GetDependencies = false) : BaseQuery, IRequest<ListRecursoProjetoViewModel>;