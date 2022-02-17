namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Sistema;

public class ListSistemaQuery : IRequest<ListSistemaViewModel>
{
    public bool GetDependencies { get; set; }
}
