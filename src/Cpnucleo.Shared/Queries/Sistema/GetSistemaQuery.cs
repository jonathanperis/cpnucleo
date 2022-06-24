namespace Cpnucleo.Shared.Queries.Sistema;

public record GetSistemaQuery(Guid Id) : BaseQuery, IRequest<GetSistemaViewModel>;