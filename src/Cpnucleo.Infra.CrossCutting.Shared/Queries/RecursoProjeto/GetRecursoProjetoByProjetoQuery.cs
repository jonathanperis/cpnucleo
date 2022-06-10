namespace Cpnucleo.Infra.CrossCutting.Shared.Queries.RecursoProjeto;

public class GetRecursoProjetoByProjetoQuery : BaseQuery, IRequest<GetRecursoProjetoByProjetoViewModel>
{
    public Guid IdProjeto { get; set; }
}
