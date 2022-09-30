namespace Cpnucleo.Shared.Queries.Recurso;

public sealed record GetRecursoQuery(Guid Id) : BaseQuery, IRequest<GetRecursoViewModel>;