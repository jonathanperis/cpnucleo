namespace Cpnucleo.Application.Queries.RecursoProjeto.GetByProjeto;

public class GetRecursoProjetoByProjetoQuery : IRequest<GetRecursoProjetoByProjetoViewModel>
{
    public Guid IdProjeto { get; set; }
}
