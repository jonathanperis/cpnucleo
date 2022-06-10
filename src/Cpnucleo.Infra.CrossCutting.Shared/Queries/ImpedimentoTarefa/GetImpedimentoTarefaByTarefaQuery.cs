namespace Cpnucleo.Infra.CrossCutting.Shared.Queries.ImpedimentoTarefa;

public class GetImpedimentoTarefaByTarefaQuery : BaseQuery, IRequest<GetImpedimentoTarefaByTarefaViewModel>
{
    public Guid IdTarefa { get; set; }
}
