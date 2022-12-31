namespace Cpnucleo.Shared.Queries.RecursoProjeto;

public sealed record ListRecursoProjetoByProjetoQuery(Guid IdProjeto) : BaseQuery, IRequest<ListRecursoProjetoByProjetoViewModel>;