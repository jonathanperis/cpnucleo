namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Sistema;

public class ListSistemaQuery : BaseQuery, IRequest<ListSistemaViewModel>
{
    public bool GetDependencies { get; set; }
}
