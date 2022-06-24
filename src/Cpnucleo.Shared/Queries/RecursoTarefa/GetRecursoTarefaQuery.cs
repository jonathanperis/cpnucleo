using Cpnucleo.Shared.Queries;

namespace Cpnucleo.Shared.Queries.RecursoTarefa;

public record GetRecursoTarefaQuery(Guid Id) : BaseQuery, IRequest<GetRecursoTarefaViewModel>;