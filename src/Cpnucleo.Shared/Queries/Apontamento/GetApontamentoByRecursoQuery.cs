namespace Cpnucleo.Infra.CrossCutting.Shared.Queries.Apontamento;

public record GetApontamentoByRecursoQuery(Guid IdRecurso) : BaseQuery, IRequest<GetApontamentoByRecursoViewModel>;