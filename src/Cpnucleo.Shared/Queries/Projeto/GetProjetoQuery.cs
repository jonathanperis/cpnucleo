namespace Cpnucleo.Shared.Queries.Projeto;

public sealed record GetProjetoQuery(Guid Id) : BaseQuery, IRequest<GetProjetoViewModel>;