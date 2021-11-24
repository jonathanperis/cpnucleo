namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Sistema;

public class ListSistemaQuery : BaseQuery, IRequest<IEnumerable<SistemaViewModel>>
{
    public bool GetDependencies { get; set; }
}