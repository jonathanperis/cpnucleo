namespace Cpnucleo.Application.Queries.Tarefa.GetByRecurso;

public class GetByRecursoQuery : IRequest<GetByRecursoViewModel>
{
    public Guid IdRecurso { get; }

    public GetByRecursoQuery(Guid idRecurso)
    {
        IdRecurso = idRecurso;
    }
}
