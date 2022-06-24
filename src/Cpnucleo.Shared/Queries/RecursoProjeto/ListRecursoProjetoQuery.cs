namespace Cpnucleo.Shared.Queries.RecursoProjeto;

public record ListRecursoProjetoQuery(bool GetDependencies = false) : BaseQuery, IRequest<ListRecursoProjetoViewModel>;