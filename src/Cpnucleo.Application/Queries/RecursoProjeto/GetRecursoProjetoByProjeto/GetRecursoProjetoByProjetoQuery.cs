namespace Cpnucleo.Application.Queries.RecursoProjeto.GetRecursoProjetoByProjeto;

public class GetRecursoProjetoByProjetoQuery : IRequest<GetRecursoProjetoByProjetoViewModel>
{
    public Guid IdProjeto { get; set; }
}
