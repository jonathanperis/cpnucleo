namespace Cpnucleo.Infra.CrossCutting.Shared.Queries.RecursoTarefa;

public record GetRecursoTarefaQuery(Guid Id) : BaseQuery, IRequest<GetRecursoTarefaViewModel>;