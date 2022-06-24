namespace Cpnucleo.Shared.Queries.Apontamento;

public record GetApontamentoByRecursoQuery(Guid IdRecurso) : BaseQuery, IRequest<GetApontamentoByRecursoViewModel>;