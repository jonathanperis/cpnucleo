namespace Cpnucleo.Infra.CrossCutting.Util.Queries.RecursoTarefa;

public class GetRecursoTarefaQuery : IRequest<GetRecursoTarefaViewModel>
{
    public Guid Id { get; set; }
}
