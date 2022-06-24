namespace Cpnucleo.Infra.CrossCutting.Shared.Queries.RecursoTarefa;

public record ListRecursoTarefaQuery(bool GetDependencies = false) : BaseQuery, IRequest<ListRecursoTarefaViewModel>;