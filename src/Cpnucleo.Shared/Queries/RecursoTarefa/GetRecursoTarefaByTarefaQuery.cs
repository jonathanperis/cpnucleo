using Cpnucleo.Shared.Queries;

namespace Cpnucleo.Shared.Queries.RecursoTarefa;

public record GetRecursoTarefaByTarefaQuery(Guid IdTarefa) : BaseQuery, IRequest<GetRecursoTarefaByTarefaViewModel>;