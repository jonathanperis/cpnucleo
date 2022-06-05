namespace Cpnucleo.Infra.CrossCutting.Util.Queries.ImpedimentoTarefa;

public class GetImpedimentoTarefaByTarefaQuery : BaseQuery, IRequest<GetImpedimentoTarefaByTarefaViewModel>
{
    public Guid IdTarefa { get; set; }
}
