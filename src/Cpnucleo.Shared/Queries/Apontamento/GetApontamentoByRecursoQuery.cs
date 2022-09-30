namespace Cpnucleo.Shared.Queries.Apontamento;

public sealed record GetApontamentoByRecursoQuery(Guid IdRecurso) : BaseQuery, IRequest<GetApontamentoByRecursoViewModel>;