namespace Cpnucleo.Infra.CrossCutting.Shared.Queries.RecursoTarefa;

public record GetRecursoTarefaByTarefaQuery(Guid IdTarefa) : BaseQuery, IRequest<GetRecursoTarefaByTarefaViewModel>;