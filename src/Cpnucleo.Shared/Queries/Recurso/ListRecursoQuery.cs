namespace Cpnucleo.Infra.CrossCutting.Shared.Queries.Recurso;

public record ListRecursoQuery(bool GetDependencies = false) : BaseQuery, IRequest<ListRecursoViewModel>;