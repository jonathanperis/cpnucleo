namespace Cpnucleo.Application.Queries.RecursoTarefa.GetRecursoTarefa;

public class GetRecursoTarefaQuery : IRequest<GetRecursoTarefaViewModel>
{
    public Guid Id { get; }

    public GetRecursoTarefaQuery(Guid id)
    {
        Id = id;
    }
}
