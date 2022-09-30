namespace Cpnucleo.Shared.Queries.Sistema;

public sealed record ListSistemaQuery(bool GetDependencies = false) : BaseQuery, IRequest<ListSistemaViewModel>;