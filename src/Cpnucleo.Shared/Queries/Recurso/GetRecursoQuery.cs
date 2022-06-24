namespace Cpnucleo.Shared.Queries.Recurso;

public record GetRecursoQuery(Guid Id) : BaseQuery, IRequest<GetRecursoViewModel>;