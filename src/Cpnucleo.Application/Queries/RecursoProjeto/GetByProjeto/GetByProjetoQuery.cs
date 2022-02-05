namespace Cpnucleo.Application.Queries.RecursoProjeto.GetByProjeto;

public class GetByProjetoQuery : IRequest<GetByProjetoViewModel>
{
    public Guid IdProjeto { get; set; }
}
