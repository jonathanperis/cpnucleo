namespace Cpnucleo.Infra.CrossCutting.Shared.Queries.Sistema;

public class ListSistemaQuery : BaseQuery, IRequest<ListSistemaViewModel>
{
    public bool GetDependencies { get; set; }
}
