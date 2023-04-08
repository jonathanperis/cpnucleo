namespace Cpnucleo.Shared.Queries.ListRecursoProjetoByProjeto;

public sealed record ListRecursoProjetoByProjetoQuery(Guid IdProjeto) : BaseQuery, IRequest<ListRecursoProjetoByProjetoViewModel>;