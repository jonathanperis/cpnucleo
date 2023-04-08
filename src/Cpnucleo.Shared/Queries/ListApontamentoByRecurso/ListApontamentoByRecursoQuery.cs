namespace Cpnucleo.Shared.Queries.ListApontamentoByRecurso;

public sealed record ListApontamentoByRecursoQuery(Guid IdRecurso) : BaseQuery, IRequest<ListApontamentoByRecursoViewModel>;