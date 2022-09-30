namespace Cpnucleo.Shared.Queries.RecursoProjeto;

public sealed record GetRecursoProjetoQuery(Guid Id) : BaseQuery, IRequest<GetRecursoProjetoViewModel>;