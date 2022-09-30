namespace Cpnucleo.Shared.Queries.Recurso;

public sealed record ListRecursoQuery(bool GetDependencies = false) : BaseQuery, IRequest<ListRecursoViewModel>;