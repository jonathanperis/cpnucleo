namespace Cpnucleo.Shared.Queries.Apontamento;

public sealed record ListApontamentoByRecursoQuery(Guid IdRecurso) : BaseQuery, IRequest<ListApontamentoByRecursoViewModel>;