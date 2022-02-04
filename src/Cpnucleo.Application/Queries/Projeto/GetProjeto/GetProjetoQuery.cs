namespace Cpnucleo.Application.Queries.Projeto.GetProjeto;

public class GetProjetoQuery : IRequest<GetProjetoViewModel>
{
    public Guid Id { get; }

    public GetProjetoQuery(Guid id)
    {
        Id = id;
    }
}
