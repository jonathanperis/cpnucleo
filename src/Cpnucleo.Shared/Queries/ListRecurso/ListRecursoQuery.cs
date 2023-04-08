namespace Cpnucleo.Shared.Queries.ListRecurso;

public sealed record ListRecursoQuery(bool GetDependencies = false) : BaseQuery, IRequest<ListRecursoViewModel>;