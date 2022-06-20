namespace Cpnucleo.Infra.CrossCutting.Shared.Queries.RecursoProjeto;

public record GetRecursoProjetoQuery(Guid Id) : BaseQuery, IRequest<GetRecursoProjetoViewModel>;