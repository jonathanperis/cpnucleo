namespace Cpnucleo.Infra.CrossCutting.Util.Queries.RecursoProjeto;

public class GetRecursoProjetoByProjetoQuery : BaseQuery, IRequest<GetRecursoProjetoByProjetoViewModel>
{
    public Guid IdProjeto { get; set; }
}
