namespace Cpnucleo.Shared.Queries.Projeto;

public record GetProjetoQuery(Guid Id) : BaseQuery, IRequest<GetProjetoViewModel>;