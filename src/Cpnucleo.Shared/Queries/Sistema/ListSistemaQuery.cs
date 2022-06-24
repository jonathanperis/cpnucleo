namespace Cpnucleo.Infra.CrossCutting.Shared.Queries.Sistema;

public record ListSistemaQuery(bool GetDependencies = false) : BaseQuery, IRequest<ListSistemaViewModel>;