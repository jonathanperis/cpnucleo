namespace Cpnucleo.Infra.CrossCutting.Shared.Queries.ImpedimentoTarefa;

public class GetImpedimentoTarefaQuery : BaseQuery, IRequest<GetImpedimentoTarefaViewModel>
{
    public Guid Id { get; set; }
}
