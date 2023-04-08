namespace Cpnucleo.Shared.Queries.GetProjeto;

public sealed record GetProjetoQuery(Guid Id) : BaseQuery, IRequest<GetProjetoViewModel>;