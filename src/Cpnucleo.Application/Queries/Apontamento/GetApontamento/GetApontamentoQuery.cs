namespace Cpnucleo.Application.Queries.Apontamento.GetApontamento;

public class GetApontamentoQuery : IRequest<GetApontamentoViewModel>
{
    public Guid Id { get; }

    public GetApontamentoQuery(Guid id)
    {
        Id = id;
    }
}
