namespace Cpnucleo.Shared.Queries.Recurso;

public record ListRecursoQuery(bool GetDependencies = false) : BaseQuery, IRequest<ListRecursoViewModel>;