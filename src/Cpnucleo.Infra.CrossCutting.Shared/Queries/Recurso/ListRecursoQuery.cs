namespace Cpnucleo.Infra.CrossCutting.Shared.Queries.Recurso;

public class ListRecursoQuery : BaseQuery, IRequest<ListRecursoViewModel>
{
    public bool GetDependencies { get; set; }
}
