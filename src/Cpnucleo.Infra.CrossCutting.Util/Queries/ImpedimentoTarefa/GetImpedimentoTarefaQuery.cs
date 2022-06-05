namespace Cpnucleo.Infra.CrossCutting.Util.Queries.ImpedimentoTarefa;

public class GetImpedimentoTarefaQuery : BaseQuery, IRequest<GetImpedimentoTarefaViewModel>
{
    public Guid Id { get; set; }
}
