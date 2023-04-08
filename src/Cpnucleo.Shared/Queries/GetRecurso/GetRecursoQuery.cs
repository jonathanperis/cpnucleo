namespace Cpnucleo.Shared.Queries.GetRecurso;

public sealed record GetRecursoQuery(Guid Id) : BaseQuery, IRequest<GetRecursoViewModel>;