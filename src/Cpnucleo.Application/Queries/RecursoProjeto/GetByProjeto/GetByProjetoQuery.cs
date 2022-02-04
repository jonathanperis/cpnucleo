namespace Cpnucleo.Application.Queries.RecursoProjeto.GetByProjeto;

public class GetByProjetoQuery : IRequest<GetByProjetoViewModel>
{
    public Guid IdProjeto { get; }

    public GetByProjetoQuery(Guid idProjeto)
    {
        IdProjeto = idProjeto;
    }
}
