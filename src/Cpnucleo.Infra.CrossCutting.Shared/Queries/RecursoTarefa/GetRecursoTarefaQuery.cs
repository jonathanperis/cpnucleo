namespace Cpnucleo.Infra.CrossCutting.Shared.Queries.RecursoTarefa;

public class GetRecursoTarefaQuery : BaseQuery, IRequest<GetRecursoTarefaViewModel>
{
    public Guid Id { get; set; }
}
