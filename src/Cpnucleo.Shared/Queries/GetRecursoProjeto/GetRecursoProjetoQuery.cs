namespace Cpnucleo.Shared.Queries.GetRecursoProjeto;

public sealed record GetRecursoProjetoQuery(Guid Id) : BaseQuery, IRequest<GetRecursoProjetoViewModel>;