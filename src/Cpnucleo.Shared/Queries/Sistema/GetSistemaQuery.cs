namespace Cpnucleo.Shared.Queries.Sistema;

public sealed record GetSistemaQuery(Guid Id) : BaseQuery, IRequest<GetSistemaViewModel>;