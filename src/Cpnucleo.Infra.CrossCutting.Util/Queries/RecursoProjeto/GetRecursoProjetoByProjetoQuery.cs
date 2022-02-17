namespace Cpnucleo.Infra.CrossCutting.Util.Queries.RecursoProjeto;

public class GetRecursoProjetoByProjetoQuery : IRequest<GetRecursoProjetoByProjetoViewModel>
{
    public Guid IdProjeto { get; set; }
}
