namespace Cpnucleo.Shared.Queries.GetSistema;

public sealed record GetSistemaQuery(Guid Id) : BaseQuery, IRequest<GetSistemaViewModel>;