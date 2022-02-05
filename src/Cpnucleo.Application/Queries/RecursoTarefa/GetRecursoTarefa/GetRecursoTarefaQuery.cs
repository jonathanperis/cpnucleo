namespace Cpnucleo.Application.Queries.RecursoTarefa.GetRecursoTarefa;

public class GetRecursoTarefaQuery : IRequest<GetRecursoTarefaViewModel>
{
    public Guid Id { get; set; }
}
