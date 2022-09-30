namespace Cpnucleo.Shared.Queries.RecursoProjeto;

public sealed record GetRecursoProjetoByProjetoQuery(Guid IdProjeto) : BaseQuery, IRequest<GetRecursoProjetoByProjetoViewModel>;