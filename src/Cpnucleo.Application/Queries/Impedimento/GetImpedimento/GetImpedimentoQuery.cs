namespace Cpnucleo.Application.Queries.Impedimento.GetImpedimento;

public class GetImpedimentoQuery : IRequest<GetImpedimentoViewModel>
{
    public Guid Id { get; }

    public GetImpedimentoQuery(Guid id)
    {
        Id = id;
    }
}
