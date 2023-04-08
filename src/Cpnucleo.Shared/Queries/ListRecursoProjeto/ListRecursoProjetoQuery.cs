namespace Cpnucleo.Shared.Queries.ListRecursoProjeto;

public sealed record ListRecursoProjetoQuery(bool GetDependencies = false) : BaseQuery, IRequest<ListRecursoProjetoViewModel>;