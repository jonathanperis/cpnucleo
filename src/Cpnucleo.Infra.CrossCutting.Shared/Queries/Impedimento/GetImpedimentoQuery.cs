namespace Cpnucleo.Infra.CrossCutting.Shared.Queries.Impedimento;

public class GetImpedimentoQuery : BaseQuery, IRequest<GetImpedimentoViewModel>
{
    public Guid Id { get; set; }
}
